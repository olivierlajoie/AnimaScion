// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : AnimationDTO.cs
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
using AnimaScion.Modele;

namespace AnimaScion.DTO
{
    /// <summary>
    /// Classe DTO pour l'affichage des animations.
    /// </summary>
	public class AnimationDTO
	{
        public string Nom { get; private set; }
        public bool Boucle { get; private set; }
        public int NombreSequence { get; private set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        /// <param name="uneAnimation"></param>
		public AnimationDTO (Animation uneAnimation)
		{
            this.Nom = uneAnimation.Nom;
            this.Boucle = uneAnimation.Boucle;
            this.NombreSequence = uneAnimation.GetNombreSequence();
		}
	}
}

