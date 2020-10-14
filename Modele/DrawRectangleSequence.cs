// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : DrawRectangleSequence.cs
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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AnimaScion.Modele
{
    public class DrawRectangle : Sequence
    {
        public int PositionY1 { get; set; }
        public int PositionY2 { get; set; }
        public bool Fill { get; set; }

        public DrawRectangle() 
            :base()
        {
            this.Nom = null;
            this.PositionX = 0;
            this.PositionY = 0;
            this.PositionY1 = 0;
            this.PositionY2 = 0;
            this.Fill = false;
        }

        public DrawRectangle(string nom, int positionX, int positionX2, int positionY, int positionY2, bool fill)
            :base(nom, positionX, positionX2)
        {
            this.Nom = nom;
            this.PositionX = positionX;
            this.PositionY = positionX2;
            this.PositionY1 = positionY;
            this.PositionY2 = positionY2;
            this.Fill = fill;
        }

        public override byte[] Afficher()
        {
            string nomSequence = this.Nom + "\0";
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes(nomSequence);
            byte[] buffer = new byte[32];

            // Nom.
            for (int i = 0; i < cmd.Length; i++)
            {
                buffer[i] = cmd[i];
            }

            buffer[27] = byte.Parse(this.PositionX.ToString());
            buffer[28] = byte.Parse(this.PositionY.ToString());
            buffer[29] = byte.Parse(this.PositionY1.ToString());
            buffer[30] = byte.Parse(this.PositionY2.ToString());
            buffer[31] = byte.Parse(Convert.ToInt32(this.Fill).ToString());

            return buffer;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}