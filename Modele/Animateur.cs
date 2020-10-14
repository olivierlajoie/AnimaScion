// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : Animateur.cs
// Date crée : 2016-4-14
// Date dern. modif. : 2016-5-1
// *******************************************************
// Historique des modifications
// *******************************************************
// 2016-4-14	 Olivier Lajoie      Version initiale.
// 2016-4-16     Olivier Lajoie      Ajout des attributs.
// 2016-4-20     Olivier Lajoie      Ajout des méthodes.
// 2016-4-25     Olivier Lajoie      Modification des méthodes.
// 2016-5-1      Olivier Lajoie      Version Finale.
// *******************************************************/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace AnimaScion.Modele
{
    /// <summary>
    /// Classe d'animateur.
    /// </summary>
    [XmlRoot("Animateur")]
	public class Animateur
	{
        private const int NOMBRE_ANIMATION_MAX = 10;
        private List<Animation> listeAnimation;

        [XmlArray("Animations")]
        public List<Animation> ListeAnimation { get { return this.listeAnimation; } set { this.listeAnimation = value; } }

        /// <summary>
        /// Constructeur par defaut.
        /// </summary>
		public Animateur()
		{
            this.listeAnimation = new List<Animation>();
		}

        /// <summary>
        /// Méthode permettant d'ajouter une animation.
        /// </summary>
        /// <param name="uneAnimation"></param>
        /// <param name="uneBoucle"></param>
        /// <returns></returns>
        public bool AjouterAnimation(Animation uneAnimation, bool uneBoucle)
        {
            if (listeAnimation.Count < NOMBRE_ANIMATION_MAX)
            {
                if (!this.IsAnimationPresent(uneAnimation))
                {
                    this.listeAnimation.Add(uneAnimation);
                    this.GetAnimation(uneAnimation).Boucle = uneBoucle;
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Méthode permettant de modifier une animation.
        /// </summary>
        /// <param name="uneAnimation"></param>
        /// <param name="nom"></param>
        /// <param name="boucle"></param>
        /// <returns></returns>
        public bool ModifierAnimation(Animation uneAnimation, string nom, bool boucle)
        {
            nom = GetNomAnimation(nom);

            this.GetAnimation(uneAnimation).Boucle = boucle;
            this.GetAnimation(uneAnimation).Nom = nom;
            return true;
        }

        /// <summary>
        /// Méthode permettant de retirer une animation.
        /// </summary>
        /// <param name="uneAnimation"></param>
        /// <returns></returns>
        public bool EnleverAnimation(Animation uneAnimation)
        {
            if (this.IsAnimationPresent(uneAnimation))
            {
                this.listeAnimation.Remove(uneAnimation);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Méthode permettant d'obtenir une animation de la liste.
        /// </summary>
        /// <param name="uneAnimation"></param>
        /// <returns></returns>
        public Animation GetAnimation(Animation uneAnimation)
        {
            return this.listeAnimation.Find(x => x.Equals(uneAnimation));
        }

        /// <summary>
        /// Méthode permettant d'obtenir la liste de toutes les animations.
        /// </summary>
        /// <returns></returns>
        public IList<Animation> GetAllAnimation()
        {
            return this.listeAnimation;
        }

        /// <summary>
        /// Méthode permettant de comparer les noms des animations.
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public string GetNomAnimation(string nom)
        {
            for (int i = 0; i < listeAnimation.Count; i++)
            {
                if (nom == listeAnimation[i].Nom)
                {
                    return nom + " (1)";
                }
            }

            return nom;
        }

        /// <summary>
        /// Méthode permettant de vérifier l'existance d'une animation dans la liste.
        /// </summary>
        /// <returns></returns>
        public bool IsAucuneAnimation()
        {
            if (this.listeAnimation.Count == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Méthode permettant de vérifier l'existance d'une animation.
        /// </summary>
        /// <returns></returns>
        private bool IsAnimationPresent(Animation uneAnimation)
        {
            return this.listeAnimation.Contains(uneAnimation);
        }

        /// <summary>
        /// Méthode permettant de vider la liste.
        /// </summary>
        public void ViderListeAnimation()
        {
            this.listeAnimation.Clear();
        }
	}
}

