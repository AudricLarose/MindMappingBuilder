import nltk
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize, sent_tokenize
from nltk.stem import PorterStemmer
from nltk.stem import WordNetLemmatizer
from gensim.corpora.dictionary import Dictionary
from nltk import * 
from string import punctuation
import zipfile
import numpy as np

nltk.download('maxent_ne_chunker')
nltk.download('stopwords')
nltk.download('punkt')
nltk.download('words')
nltk.download('averaged_perceptron_tagger')
nltk.download('wordnet')
nltk.download('omw-1.4')

textimported= sys.argv[1]
stopword=set(stopwords.words('french'))

word_tokens=word_tokenize(textimported)
phrase = [w for w in word_tokens if not w in stopword]
tagged=nltk.pos_tag(phrase)
chunk_ne=nltk.ne_chunk(tagged)
stopwordpunc=stopword.union(set(punctuation))
phrase = [w.lower() for w in word_tokens if not w in stopwordpunc]
lemmatizer=WordNetLemmatizer()
language=[]
for word in phrase :
  language.append(lemmatizer.lemmatize(word.lower()))
searchin=re.compile(r'\s[a-zA-Z1-9][a-zA-Z1-9]?[1-9]?\s')
searchch=searchin.finditer(textimported)
lilstopword=[]
for search in searchch :
  lilstopword.append(search.group().strip())
phrase1=[word for word in phrase if not word in lilstopword ]
langage2=[langue for langue in language if langue in phrase1]
searchin=re.compile(r'\s[a-zA-Z1-9][a-zA-Z1-9]?[1-9]?\s')
searchch=searchin.finditer(textimported)
grammar_rules1=r"NP: {<DT>?<JJ>*<NN>}"
chunk_parser=nltk.RegexpParser(grammar_rules1)
chunk_result=chunk_parser.parse(tagged)
print(langage2)
