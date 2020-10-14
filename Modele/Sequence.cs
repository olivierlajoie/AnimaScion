// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : Sequence.cs
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
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AnimaScion.Modele
{
    /// <summary>
    /// Classe abstraite d'une séquence.
    /// </summary>
    [XmlInclude(typeof(Shift))]
    [XmlInclude(typeof(Delay))]
    [XmlInclude(typeof(DisplayCache))]
    [XmlInclude(typeof(SetColor))]
    [XmlInclude(typeof(SetCache))]
    [XmlInclude(typeof(ChangeBrightness))]
    [XmlInclude(typeof(DrawFromSD))]
    [XmlInclude(typeof(DrawRectangle))]
    [XmlInclude(typeof(DrawCircle))]
    [XmlInclude(typeof(DrawX))]
    [XmlInclude(typeof(SendBuffer))]
    [XmlInclude(typeof(Sequence))]
    public abstract class Sequence
    {
        public string Nom { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        /// <summary>
        /// Constructeur par defaut.
        /// </summary>
        public Sequence() 
        {
            this.Nom = null;
            this.PositionX = 0;
            this.PositionY = 0;
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Sequence(string nom, int positionX, int positionY)
        {
            this.Nom = nom;
            this.PositionX = positionX;
            this.PositionY = positionY;
        }

        public abstract byte[] Afficher();

        /// <summary>
        /// Méthode d'override ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Nom;
        }

        /// <summary>
        /// Méthode d'override Equals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return ((this.Nom.Equals((obj as Sequence).Nom)));
        }

        /// <summary>
        /// Méthode d'override GetHashCode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (1 * this.Nom.Length);
        }
    }
}