using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// /// <author>Ronan GODICHEAU--TORNIER</author>
    /// </summary>
    class Devise
    {
        private String vNom;
        private Dictionary<Devise, double> vDevisesLiees;
        private Devise vAntecedent;
        private int vPoids;
        private Boolean vParcouru;
        public Devise(string nom)
        {
            this.vNom = nom;
            this.vDevisesLiees = new Dictionary<Devise, double>();
            this.vPoids = 0;
        }

        /**
         * Getter du nom de la devise
         * 
         * Retour : le nom de la devise
         */
        public String getNom()
        {
            return vNom;
        }

        /**
         * Ajout d'une devise avec laquelle il y a une conversion directe, et du taux de cette conversion, dans un dictionnaire. 
         * 
         * Paramètres : 
         *  - aDevise : Devise 
         *  - aTauxConversion : taux de la conversion. 
         *  
         * Retour :
         *  - Vide
         */
        public void addDevise(Devise aDevise, double aTauxConversion)
        {
            vDevisesLiees.Add(aDevise, aTauxConversion);
        }

        /**
         * Setter de l'antécédent de la devise
         * Paramètres : 
         *  - aAntecedent : Antécédent à affecter à la devise
         *  
         * Retour : Vide
         */
        public void setAntecedent(Devise aAntecedent)
        {
            this.vAntecedent = aAntecedent;
        }

        /**
        * Setter du poids de la devise
        * Paramètres : 
        *  - aPoids : Poids à affecter à la devise
        *  
        * Retour : Vide
        */
        public void setPoids(int aPoids)
        {
            this.vPoids = aPoids;
        }

        /**
        * Getter de l'antécédent de la devise
        * 
        * Retour : Antécédent de la devise
        */
        public Devise getAntecedent()
        {
            return this.vAntecedent;
        }

        /**
        * Getter du poids de la devise
        * 
        * Retour : Poids de la devise
        */
        public int getPoids()
        {
            return this.vPoids;
        }

        /**
         * Méthode retournant toutes les devises contenues dans le dictionnaire vDevisesLiees (Devises ayant une conversion directe avec la devise actuelle)
         *  
         * Retour : Liste contenant les devises
         */
        public IList<Devise> getDevisesLiees()
        {

            IList<Devise> vListeDevises = new List<Devise>();
            foreach (KeyValuePair<Devise, double> cles in vDevisesLiees)
            {
                vListeDevises.Add(cles.Key);
            }
            
            return vListeDevises;
        }

        /**
        * Getter du taux de conversion entre la devise actuelle et la devise passée en paramètre
        * 
        * Paramètres : 
        *  - aDevise : Devise pour laquelle on veut connaitre le taux de conversion
        *  
        * Retour : Double qui correspond au taux de la conversion
        */
        public double getTaux(Devise aDevise)
        {

           vDevisesLiees.TryGetValue(aDevise, out double vTauxConversion);
            return vTauxConversion;
            
        }

        /**
         * Méthode affectant à la devise un booléen pour savoir si la devise a été parcourue dans l'algorithme de Djikstra 
         * 
         * Paramètres : 
         *  - aParcouru : booléen
         *  
         * Retour: Vide
         */
        public void setParcouru(bool aParcouru)
        {
            this.vParcouru = aParcouru;
        }

        /**
         * Méthode qui retourne si la devise a été parcourue dans l'algorithme de Djikstra
         * 
         * Retour: booléen qui est true si la devise a été parcourue et qui est false dans le cas contraire
         */
        public Boolean isParcouru()
        {
            return vParcouru;
        }
    }
}
