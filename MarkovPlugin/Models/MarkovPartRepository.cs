using LimeBeanEnhancements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeBean.Interfaces;

namespace MarkovPlugin.Models
{
    class MarkovPartRepository : BeanRepository<MarkovPart>
    {
        public MarkovPartRepository(IBeanAPI beanApi) : base(beanApi)
        {
            
        }

        public MarkovPart LoadPart(string wordBefore, string word, string wordAfter)
        {
            try
            {
                return _beanApi.FindOne<MarkovPart>(true, "WHERE word_before = {0} AND word = {1} AND word_after = {2}", wordBefore, word, wordAfter);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Exists(string wordBefore, string word, string wordAfter)
        {
            return LoadPart(wordBefore, word, wordAfter) != null ? true : false;
        }

        private Word GetWord(string word, bool create = true)
        {
            if (String.IsNullOrWhiteSpace(word))
                return null;

            Word returnWord = null;

            try
            {
                returnWord = _beanApi.FindOne<Word>(true, "WHERE value = {0}", word);
                if (returnWord == null) throw new Exception("Word NULL");
            }
            catch (Exception ex)
            {
                if (create)
                {
                    Word newWord = _beanApi.Dispense<Word>();
                    newWord.Value = word;
                    long id = (long)_beanApi.Store(newWord);
                    returnWord = _beanApi.Load<Word>(id);
                }
            }

            return returnWord;
        }

        internal void AddSentence(string text)
        {
            try
            {
                text = text.Replace("'", "").Replace("`", "");

                List<string> words = new List<string>(text.Split(' '));
                words.RemoveAll(string.IsNullOrWhiteSpace);

                for (int i = 0; i <= words.Count - 1; i++)
                {
                    string wordBefore = "";
                    string word = "";
                    string wordAfter = "";

                    word = words[i];

                    if (i > 0)
                    {
                        wordBefore = words[i - 1];
                    }

                    if (i < words.Count - 1)
                    {
                        wordAfter = words[i + 1];
                    }

                    Insert(wordBefore, word, wordAfter);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Insert(string wordBefore, string word, string wordAfter)
        {
            MarkovPart part = LoadPart(wordBefore, word, wordAfter);

            if (part != null)
            {
                part.Count++;
            }
            else
            {
                part = Create();
                part.WordBefore = GetWord(wordBefore);
                part.Word = GetWord(word);
                part.WordAfter = GetWord(wordAfter);
            }

            Save(part);
        }

        public string GetSentence(string startWord)
        {
            string returnText = String.Empty;

            Word word = GetWord(startWord);

            if (word != null)
            {
                MarkovPart startPart = GetNextPart(0, word.Id);
                if (startPart != null)
                {
                    returnText += startPart.Word.Value + " ";

                    MarkovPart part = startPart;

                    long currentId = part.Word.Id;
                    long nextId = part.WordAfter != null ? part.WordAfter.Id : 0;

                    while (part.WordAfter != null)
                    {
                        part = GetNextPart(currentId, nextId);

                        if (part == null)
                            break;

                        returnText += part.Word.Value + " ";

                        currentId = part.Word.Id;
                        nextId = part.WordAfter != null ? part.WordAfter.Id : 0;
                    }

                    return returnText;
                }
            }

            return null;
        }

        private MarkovPart GetNextPart(long wordBeforeId, long wordId)
        {
            var row = _beanApi.Row(false, "SELECT id FROM (SELECT * FROM `markovpart` WHERE `word_before`= {0} AND `word`= {1} ORDER BY `count` DESC LIMIT 5) AS part ORDER BY RANDOM() LIMIT 1;", wordBeforeId, wordId);

            if (row != null)
            {
                long id = (long)row["id"];
                return Get(id);
            }

            return null;
        }
    }
}
