using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using edu.stanford.nlp.parser;
using java.util;
using edu.stanford.nlp.ie.crf;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using System.IO;
using edu.stanford.nlp.ling;

namespace EntitiesMapper
{
    /// <summary>
    /// Apply Stanford NLP parser to the input web document test collection
    /// </summary>
    /// <packages needed>
    /// 1. Stanford.NLP.CoreNLP
    /// 2. IKVM
    /// </packages needed>
    class Program
    {
        public static CRFClassifier Classifier =
            CRFClassifier.getClassifierNoExceptions(
                 @"C:\english.all.3class.distsim.crf.ser.gz");

        // For either a file to annotate or for the hardcoded text example,
        // this demo file shows two ways to process the output, for teaching
        // purposes.  For the file, it shows both how to run NER on a String
        // and how to run it on a whole file.  For the hard-coded String,
        // it shows how to run it on a single sentence, and how to do this
        // and produce an inline XML output format.

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var fileContent = File.ReadAllText(args[0]);
                foreach (List sentence in Classifier.classify(fileContent).toArray())
                {
                    foreach (CoreLabel word in sentence.toArray())
                    {
                        Console.Write("{0}/{1} ", word.word(), word.get(new CoreAnnotations.AnswerAnnotation().getClass()));
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                const string S1 = "Good afternoon Rajat Raina, how are you today? Good afternoon Anil Raj and Mr. Bean how are you today?";
                const string S2 = "I go to school at Stanford University, which is located in California.";
                Console.WriteLine("{0}\n", Classifier.classifyToString(S1));
                Console.WriteLine("{0}\n", Classifier.classifyWithInlineXML(S2));
                Console.WriteLine("{0}\n", Classifier.classifyToString(S2, "xml", true));

                var classification = Classifier.classify(S2).toArray();
                for (var i = 0; i < classification.Length; i++)
                {
                    Console.WriteLine("{0}\n:{1}\n", i, classification[i]);
                }
                Console.WriteLine("The second method output\n");

                var classified = Classifier.classifyToCharacterOffsets(S1).toArray();

                for (int i = 0; i < classified.Length; i++)
                {
                    Triple triple = (Triple)classified[i];

                    int second = Convert.ToInt32(triple.second().ToString());
                    int third = Convert.ToInt32(triple.third().ToString());

                    Console.WriteLine("docNo" + '\t' + triple.first().ToString() + '\t' + S1.Substring(second, third - second));
                }
            }
            Console.ReadLine();
        }
    }
    //class Program
    //{
    //    public static CRFClassifier Classifier =
    //        CRFClassifier.getClassifierNoExceptions(@"C:\english.all.3class.distsim.crf.ser.gz");
    //    //CRFClassifier.getClassifierNoExceptions(
    //    //    @"english.all.3class.distsim.crf.ser.gz");

    //    static void Main(string[] args)
    //    {

    //        string line = null;

    //        while ((line = Console.ReadLine()) != null)
    //        {
    //            try
    //            {
    //                // Input as docNo, url, domain, contentlength, tokens
    //                string[] rows = line.Split('\t');

    //                string docNo = rows[0];
    //                string tokens = rows[4];
    //                string docText = rows[5];

    //                var classified = Classifier.classifyToCharacterOffsets(docText).toArray();

    //                for (int i = 0; i < classified.Length; i++)
    //                {
    //                    Triple triple = (Triple)classified[i];

    //                    int second = Convert.ToInt32(triple.second().ToString());
    //                    int third = Convert.ToInt32(triple.third().ToString());

    //                    Console.WriteLine(docNo + '\t' + triple.first().ToString() + '\t' + docText.Substring(second, third - second));
    //                }
    //            }
    //            catch
    //            {
    //                // Error silently
    //            }
    //        }
    //    }
    //}
}
