// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : DrawCircleSequence.cs
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
    public class DrawCircle : Sequence
    {
        public int Size { get; set; }

        public DrawCircle() 
            :base()
        {
            this.Nom = null;
            this.PositionX = 0;
            this.PositionY = 0;
        }

        public DrawCircle(string nom, int positionX, int positionY, int size)
            :base(nom, positionX, positionY)
        {
            this.Nom = nom;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.Size = size;
        }

        public override byte[] Afficher()
        {
            string nomSequence = this.Nom + "\0";
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes(nomSequence);
            byte[] buffer = new byte[30];

            // Nom.
            for (int i = 0; i < cmd.Length; i++)
            {
                buffer[i] = cmd[i];
            }

            buffer[27] = byte.Parse(this.PositionX.ToString());
            buffer[28] = byte.Parse(this.PositionY.ToString());
            buffer[29] = byte.Parse(this.Size.ToString());

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