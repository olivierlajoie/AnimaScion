// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : SequenceDTO.cs
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
    /// Classe DTO pour l'affichage des séquences.
    /// </summary>
    public class SequenceDTO
    {
        public string Nom { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        /// <param name="uneSequence"></param>
        public SequenceDTO(Sequence uneSequence)
        {
            this.Nom = uneSequence.Nom;
            this.PositionX = uneSequence.PositionX;
            this.PositionY = uneSequence.PositionY;
        }
    }
}

