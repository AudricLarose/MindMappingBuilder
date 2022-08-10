using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace MindMappingBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string temp = "";
        int clicked = 0;
        bool precheck=false;
        bool confirmed=false;
        List<KeyWord> BoW= new List<KeyWord>();
        ObservableCollection<Branche> racine = new ObservableCollection<Branche>();
        bool over =false;
        int[] cypher = { 12, 15, 557, 42, 158, 1547, 24148, 324 };
        Branche branche_navette;
        List<theKooples> couples= new List<theKooples>();
        private int iterator;

        public string couple { get; set; }

        class KeyWord
        {
            public string mot { get; set; }
            public int nbrApparition;
            public int importance;
            public int poid;
            public List<string> getRelation(List<theKooples> couples)
            {
                List<string> relation = new List<string>();
                foreach (theKooples couple in couples)
                {
                    if (couple.couple.Contains(mot))
                    {
                        relation.Add(couple.couple);
                    }
                }
                return relation;
            }
        }
        class theKooples
        {
            public string couple { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }



    private void Bouton_Click_1(object sender, RoutedEventArgs e)
        {
            string g = text_box.Text;
            List<string> chunk_of_100_words=chunk_words(g);
            bandeau_information.Text = "Traitement en cours";
            Bouton.Content = "Traitement en cours";
            //howManyWord();
            
            for (int i = 0; i < chunk_of_100_words.Count; i++)
            {
                NaturalProgression.Maximum = chunk_of_100_words.Count();
                reeNaturalProgression.Value = i;
                string textOpropre = cleaning_engennering(Option1_ExecProcess(chunk_of_100_words[i]));
                foreach (string data in creer_tableau(textOpropre))
                {
                    BoW.Add(new KeyWord() { mot = Majuscule(data.Trim()) });
                }
            }
            bandeau_information.Text = "Nombre de mots Total : " + countAllWord(g) + " dont " + countAllSingleWord(g) + " mots différents";
            LB_mot.ItemsSource = BoW;
        }

        private List<string> chunk_words(string text)
        {
            int iterator = 0;
            List<string> words = new List<string>();
            string creator = " ";
           string [] chunk=text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string phrase in chunk)
            {
                iterator += 1;
                if (iterator % 100==0)
                {
                    words.Add(creator);
                    creator = " ";
                } else
                {
                    creator = creator + " " + phrase;

                }
            }
            words.Add(creator);

            return words;
        } 


        private  int countAllSingleWord(string g)
        {
            List<string> duplicata = new List<string>(g.Split(" "));
            HashSet<string> hashWithoutDuplicates = new HashSet<string>(duplicata);
            List<string> listWithoutDuplicates = hashWithoutDuplicates.ToList();
            string dataz = "";
            foreach(string data in listWithoutDuplicates)
            {
                string datax = data.Replace("\n"," ");
                string dataxz = datax.Replace("\r"," ");
                if (dataxz.Length > 2)
                {
                    dataz = dataz + dataxz + " ";
                }
            }      
            return countAllWord(dataz);
        }

        private int countAllWord(string text)
        {
            Regex regex = new Regex("\\w+"  , RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection match = regex.Matches(text);
            return match.Count;
            
        }
        
        private string Majuscule (string mot)
        {
            string newstring = mot;
            try
            {
                newstring = char.ToUpper(mot[0]) + mot.Substring(1);
            } catch (Exception e)
            {

            }
            return newstring;
        }
        public string cleaning_engennering(string text)
        {
            text = text.Replace("'", " ");
            text = text.Replace("\"", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            return text;
        }

        public List<string> creer_tableau(string text)
        {       
            List<string> data = new List<string>(
                text.Split(",")
                );
            return data;            
        }

        static string Option1_ExecProcess(string text)
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\audri\AppData\Local\Programs\Python\Python38\python.exe";

            // 2) Provide script and arguments
            var script = @"C:\Users\audri\source\repos\MindMappingBuilder\MindMappingBuilder\Python\MindMappingBuilder.py";
            var start = text;

            psi.Arguments = $"\"{script}\" \"{start}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // 5) Display output
            Console.WriteLine("ERRORS:");
            Console.WriteLine(errors);
            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine(results);

            return results;

        }

        private void LB_mot_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int item = LB_mot.SelectedIndex;
            temp= BoW[item].mot;
            if (branche_navette==null)
            {
                branche_navette = new Branche() { key = 0, value = temp, feuille = new ObservableCollection<Feuille>(), sous_groupe = new ObservableCollection<Branche>() };
            }
            else
            {
                iterator = iterator == null ? iterator = 0 : iterator += 1;
                branche_navette.sous_groupe.Add(new Branche() { key = iterator, value = temp, sous_groupe=new ObservableCollection<Branche>() });
            }

        }

        private void slice_and_diced_array(IList<Branche> branches, int start, int end, string value)
        {
            if (!over) { 
            if (end - start < 1) return;
            end = end - 1;
            slice_and_diced_array(branches, start, end, value);
            compareString(branches, end, value);
        }
        }

        //{ 12, 15, 557, 42, 158, 1547, 24148, 324 };

        private void compareString(IList<Branche> branches,int end, string value)
        {
          if (!over)
          if ((branches[end]!=null) & (branches[end].value != value))
            {
                if (branches[end].sous_groupe.Count() > 1)
                {
                    slice_and_diced_array(branches[end].sous_groupe, 0, branches[end].sous_groupe.Count(), value);
                } 
                } else
            {
                foreach (Branche branche in branche_navette.sous_groupe)
                    {
                        branches[end].sous_groupe.Add(branche);
                    }
                    over =true;
            }
        }

        private void methode1()
        {
            int item = LB_mot.SelectedIndex;
            clicked = clicked + 1;
            switch (clicked)
            {
                case 1:
                    temp = BoW[item].mot;
                    break;
                case 2:
                    couples.Add(new theKooples() { couple = temp + " " + BoW[item].mot });
                    //LB_couple.Items.Add(couples[couples.Count - 1]);
                    clicked = 0;
                    break;
            }        
        }

        private void valider_Click(object sender, RoutedEventArgs e)
        {
            if (racine.Count() != 0)
            {
                if (branche_navette != null)
                {
                    slice_and_diced_array(racine, 0, racine.Count(), branche_navette.value);
                    if (!over)
                    {
                        racine.Add(branche_navette);
                    }
                    else
                    {
                        over = false;
                    }
                }
            } else
            {
                racine.Add(branche_navette);
            }
            branche_navette = null;
            GroupView.ItemsSource = racine;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
