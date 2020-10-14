// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : SendBufferSequence.cs
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
    public class SendBuffer : Sequence
    {
        public int TailleX { get; set; }
        public int TailleY { get; set; }
        public byte[] ListeLed { get; set; }

        public SendBuffer() 
            :base()
        {
            this.Nom = null;
            this.PositionX = 0;
            this.PositionY = 0;
            this.TailleX = 0;
            this.TailleY = 0;
            this.ListeLed = null;
        }

        public SendBuffer(string nom, int positionX, int positionY, int tailleX, int tailleY, byte[] listeLed)
            : base(nom, positionX, positionY)
        {
            this.Nom = nom;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.TailleX = tailleX;
            this.TailleY = tailleY;
            this.ListeLed = listeLed;
        }

        public override byte[] Afficher()
        {
            string nomSequence = this.Nom + "\0";
            byte[] cmd = ASCIIEncoding.ASCII.GetBytes(nomSequence);
            byte[] buffer = new byte[32 + this.TailleX * this.TailleY * 3];

            // Nom.
            for (int i = 0; i < cmd.Length; i++)
            {
                buffer[i] = cmd[i];
            }

            buffer[27] = byte.Parse(this.PositionX.ToString());
            buffer[28] = byte.Parse(this.PositionY.ToString());
            buffer[29] = byte.Parse(this.TailleX.ToString());
            buffer[30] = byte.Parse(this.TailleY.ToString());

            for (int i = 0; i < ListeLed.Length; i++)
            {
                buffer[i + 32] = ListeLed[i];
            }

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