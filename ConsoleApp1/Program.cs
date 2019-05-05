using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace ConsoleApp1
{
    /// <summary>
    /// /// <author>Ronan GODICHEAU--TORNIER</author>
    /// </summary>
    class Program
    {
        
        private static String vDeviseDepart, vDeviseFin;
        private static int vValeurDepart;
        private static List<Devise> vListeDevises;
        private static Dictionary<Devise, int> vMapDevisesPoids;
        private static IList<Devise> vCheminLePlusCourt;
        
        static void Main(string[] args)
        {
            vListeDevises = new List<Devise>();
            vCheminLePlusCourt = new List<Devise>();
            vMapDevisesPoids = new Dictionary<Devise, int>();

            //Chemin du fichier à utilisé, passé en paramètre de l'appel
            String vCheminFichier =  args[0];

            //Import du fichier texte et traitement de son contenu 
            ImportFichierTexte(vCheminFichier);

            //Recherche du chemin le plus court de la devise de départ à la devise d'arrivée.
            TrouverCheminPlusCourt(GetDeviseByNom(vDeviseDepart), GetDeviseByNom(vDeviseFin));

            //Conversion et affichage du montant souhaité dans la devise cible.
            CalculerValeurFinale(vValeurDepart);
        }

        /**
         * Méthode permettant de trouver le chemin le plus court pour aller de la devise A à la devise B. 
         * Les devises du chemin le plus court sont ajoutées dans la variable vCheminLePlusCourt. 
         * 
         * Cette méthode s'inspire de l'algorithme de Djikstra permettant de trouver le plus court chemin pour un graphe. 
         * 
         * Paramètres : 
         *  - aDeviseDepart : La devise A à partir de laquelle le chemin doit commencer. 
         *  - aDeviseFin : La devise B par laquelle le chemin doit se terminer. 
         *  
         * Retour
         *  - Vide
         */
        public static void TrouverCheminPlusCourt(Devise aDeviseDepart, Devise aDeviseFin)
        {
            //Initialisation des poids et des antécédents de chacune des devises. 
            Init_Poids_Antecendents(aDeviseDepart);

            Devise vDeviseActuelle = aDeviseDepart;
            IList<Devise> vDevisesVoisines = new List<Devise>();
            while (null == aDeviseFin.getAntecedent())
            {
                vDeviseActuelle.setParcouru(true);
                vDevisesVoisines = vDeviseActuelle.getDevisesLiees();
                foreach (Devise iDeviseVoisine in vDevisesVoisines)
                {
                    if (!iDeviseVoisine.isParcouru())
                    {
                        if (iDeviseVoisine.getPoids() == -1 || (vDeviseActuelle.getPoids() + 1) < iDeviseVoisine.getPoids())
                        {
                            iDeviseVoisine.setAntecedent(vDeviseActuelle);
                            iDeviseVoisine.setPoids(vDeviseActuelle.getPoids() + 1);
                            vMapDevisesPoids[iDeviseVoisine] = vDeviseActuelle.getPoids() + 1;
                        }
                    }
                }
                foreach (var devise in vMapDevisesPoids.OrderBy(i => i.Value)) // si tri par poids 
                {
                    if (devise.Key.getPoids() != -1 && !devise.Key.isParcouru())
                    {

                        vDeviseActuelle = devise.Key;
                        break;
                    }
                }
            }
            
            vCheminLePlusCourt.Add(aDeviseFin);
            
            //Réutilisation de la variable vDeviseActuelle pour définir le chemin à prendre à partir des antécédents
            vDeviseActuelle = aDeviseFin.getAntecedent();
            while (!vCheminLePlusCourt.Contains(aDeviseDepart))
            {
                vCheminLePlusCourt.Add(vDeviseActuelle);
                vDeviseActuelle = vDeviseActuelle.getAntecedent();
            }
            vCheminLePlusCourt = vCheminLePlusCourt.Reverse().ToList();
            
        }


        /** Méthode convertissant un montant dans une devise souhaitée à partir d'une autre devise. 
         *  
         *  Paramètres : 
         *   - aMontantDepart : Montant à convertir. 
         *   
         *  Retour : 
         *   - vide
         */
        private static void CalculerValeurFinale(double aMontantDepart)
        {
            double vMontantEnCours = aMontantDepart;

            //Pour chaque devise du chemin à parcourir, le montant est multiplié par le taux de la conversion. 
            for(int i =0; i <= vCheminLePlusCourt.Count-2; i++)
            {
                vMontantEnCours = Math.Round(vCheminLePlusCourt[i].getTaux(vCheminLePlusCourt[i+1]) * vMontantEnCours, 4);
            }

            double vMontantFinal = vMontantEnCours;

            //Affichage du montant final dans une nouvelle ligne sur la console. 
            Console.WriteLine(Math.Ceiling((Decimal)vMontantEnCours));
        }

        /** Méthode initialisant les poids et les antécédents de chacune des devises. 
         *  Les valeurs des antécédents sont initialisés à null. 
         *  Les valeurs des poids sont initialisés à 0 pour la devise de départ et à -1 pour les autres devises.  
         *  
         *  Paramètres : 
         *   - aDeviseDepart : Devise à partir de laquelle la conversion va commencer. 
         *   
         *  Retour : 
         *   - vide
         */
        private static void Init_Poids_Antecendents(Devise aDeviseDepart)
        {
            foreach (Devise iDevise in vListeDevises)
            {
                iDevise.setPoids(-1);
                iDevise.setAntecedent(null);
                iDevise.setParcouru(false);
                vMapDevisesPoids.Add(iDevise, -1);
            }
            aDeviseDepart.setPoids(0);
            vMapDevisesPoids[aDeviseDepart] = 0;
        }

        /** 
         * Méthode permettant retournant une devise de la liste "vListeDevises" à partir de son nom passé en paramètre. 
         * 
         * Paramètres : 
         * - aNomDevise : Nom de la devise à récupérer.
         * 
         * Retour :
         * - Si une devise a été trouvée, la devise correspondant au nom, sinon null
         */
        private static Devise GetDeviseByNom(String aNomDevise)
        {
            foreach (Devise iDevise in vListeDevises)
            {
                if (iDevise.getNom().Equals(aNomDevise))
                {
                    return iDevise;
                }
            }
            return null;
        }


        /** 
         * Méthode qui importe, lit, et traite le contenu d'un fichier texte dont le chemin est passé en paramètre. 
         * 
         * Paramètres : 
         *  - aChemin : Chemin du fichier à importer. 
         * 
         * Retour : 
         *  - vide
         */
        public static void ImportFichierTexte(String aChemin)
        {
            //lecture et affectation du contenu du fichier dans la variable 
            string vContenuFichier = System.IO.File.ReadAllText(@aChemin);
            int vNumeroLigne = 0;
            int vNumeroColonne;
            String vNomDevise1="";
            String vNomDevise2="";
            Devise vDevise1;
            Devise vDevise2;
            double vTauxConversion;

            //Pour chaque ligne du fichier.
            foreach (string row in vContenuFichier.Replace("\n", "").Split('\r'))
            { 
                vNumeroColonne = 0;
                //Si la ligne est la ligne 2 ou plus
                if (vNumeroLigne > 1)
                {
                    //Pour chaque colonne de la ligne
                    foreach (string cell in row.Split(';'))
                    {
                        //Selon le numéro de la colonne, le traitement sera différent
                        switch (vNumeroColonne)
                        {
                            case 0:
                                vNomDevise1 = cell;
                                //Vérification de l'existence de la devise dans la liste des devises. Si elle est déja présente, on ne l'importe pas. 
                                if (!vListeDevises.Contains(GetDeviseByNom(cell)))
                                {
                                    vListeDevises.Add(new Devise(cell));
                                }
                                break;
                            case 1:
                                vNomDevise2 = cell;
                                //Vérification de l'existence de la devise dans la liste des devises. Si elle est déja présente, on ne l'importe pas. 
                                if (!vListeDevises.Contains(GetDeviseByNom(cell)))
                                {
                                    vListeDevises.Add(new Devise(cell));
                                }
                                break;
                            case 2:
                                vTauxConversion = Double.Parse(cell.Replace('.', ','));
                                vDevise1 = GetDeviseByNom(vNomDevise1);
                                vDevise2 = GetDeviseByNom(vNomDevise2);

                                //Ajout des taux de conversion pour les deux devises
                                vDevise1.addDevise(vDevise2, vTauxConversion);
                                vDevise2.addDevise(vDevise1, 1 / vTauxConversion);
                                break;
                        }

                        //Incrémentation du numéro de colonne
                        vNumeroColonne++;
                    }
                }

                /*
                Pour la première ligne du fichier, sont extraites et affectées à des variables : 
                  -la devise de départ
                  -la valeur de départ 
                  -la devise de fin
                */
                else if (vNumeroLigne == 0)
                {
                    //Pour chaque colonne de la ligne
                    foreach (string cell in row.Split(';'))
                    {
                        //Selon le numéro de la colonne, le traitement sera différent
                        switch (vNumeroColonne)
                        {
                            case 0:
                                vDeviseDepart = cell;
                                break;
                            case 1:
                                vValeurDepart = Int32.Parse(cell);
                                break;
                            case 2:
                                vDeviseFin = cell;
                                break;
                        }
                        //Incrémentation du numéro de colonne
                        vNumeroColonne++;
                    }
                }
                //Incrémentation du numéro de ligne
                vNumeroLigne++;
            }
        }
    }        
}

