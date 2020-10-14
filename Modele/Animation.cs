// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : Animation.cs
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
    /// Classe d'animation.
    /// </summary>
    [XmlRoot("Animation")]
	public class Animation
	{
        private const int NOMBRE_SEQUENCE_MAX = 10;
        private List<Sequence> listeSequence;
        public string Nom { get; set; }
        public bool Boucle { get; set; }

        [XmlArray("Sequences")]
        [XmlArrayItem(typeof(Sequence))]
        public List<Sequence> ListeSequence { get { return this.listeSequence; } set { this.listeSequence = value; } }

        /// <summary>
        /// Constructeur par defaut.
        /// </summary>
		public Animation()
		{
            this.Nom = null;
            this.Boucle = true;
            this.listeSequence = new List<Sequence>();
		}

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="nom"></param>
        public Animation(string nom)
        {
            this.Nom = nom;
            this.listeSequence = new List<Sequence>();
            this.listeSequence.Add(new SendBuffer("Choisir Séquence", 0, 0, 0, 0, null));
        }

        /// <summary>
        /// Méthode permettant d'obtenir toutes les séquences.
        /// </summary>
        /// <returns></returns>
        public IList<Sequence> GetAllSequence()
        {
            return this.listeSequence;
        }

        /// <summary>
        /// Méthode permettant d'obtenir toutes les séquences.
        /// </summary>
        /// <returns></returns>
        public IList<Sequence> GetEnvoieSequence()
        {
            List<Sequence> maliste = this.listeSequence;
            maliste.Remove(new SendBuffer("Choisir Séquence", 0, 0, 0, 0, null));
            return maliste;
        }

        /// <summary>
        /// Méthode permettant d'ajouter une séquence.
        /// </summary>
        /// <param name="uneSequence"></param>
        /// <returns></returns>
        public bool AjouterSequence(Sequence uneSequence)
        {
            if (listeSequence.Count <= NOMBRE_SEQUENCE_MAX)
            {
                this.listeSequence.Add(uneSequence);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de modifier une séquence.
        /// </summary>
        /// <param name="uneSequence"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="selection"></param>
        /// <param name="nomFichier"></param>
        /// <returns></returns>
        public bool ModifierSequence(string nom, byte[] param, bool selection, string nomFichier)
        {
            SendBuffer uneSequence = new SendBuffer(nom, 0, 0, 0, 0, null);

            if (this.IsSequencePresent(uneSequence))
            {
                switch (uneSequence.Nom)
                {
                    case "SendBuffer": 
                    {
                        SendBuffer sequence = (this.GetSequence(new SendBuffer(uneSequence.Nom, 0, 0, 0, 0, null)) as SendBuffer);
                        sequence.PositionX = int.Parse(param[0].ToString());
                        sequence.PositionY = int.Parse(param[1].ToString());
                        sequence.TailleX = int.Parse(param[2].ToString());
                        sequence.TailleY = int.Parse(param[3].ToString());
                        break;
                    }

                    case "Shift":
                    {
                        Shift sequence = (this.GetSequence(new Shift(uneSequence.Nom, 0, 0, false)) as Shift);
                        sequence.PositionX = int.Parse(param[0].ToString());
                        sequence.PositionY = int.Parse(param[1].ToString());
                        sequence.Rotation = selection;
                        break;
                    }

                    case "DrawRectangle":
                    {
                        DrawRectangle sequence = (this.GetSequence(new DrawRectangle(uneSequence.Nom, 0, 0, 0, 0, false)) as DrawRectangle);
                        sequence.PositionX = int.Parse(param[0].ToString());
                        sequence.PositionY = int.Parse(param[1].ToString());
                        sequence.PositionY1 = int.Parse(param[2].ToString());
                        sequence.PositionY2 = int.Parse(param[3].ToString());
                        sequence.Fill = selection;
                        break;
                    }

                    case "DrawX":
                    {
                        DrawX sequence = (this.GetSequence(new DrawX(uneSequence.Nom, 0, 0, 0)) as DrawX);
                        sequence.PositionX = int.Parse(param[0].ToString());
                        sequence.PositionY = int.Parse(param[1].ToString());
                        sequence.Size = int.Parse(param[2].ToString());
                        break;
                    }

                    case "DrawCircle":
                    {
                        DrawCircle sequence = (this.GetSequence(new DrawCircle(uneSequence.Nom, 0, 0, 0)) as DrawCircle);
                        sequence.PositionX = int.Parse(param[0].ToString());
                        sequence.PositionY = int.Parse(param[1].ToString());
                        sequence.Size = int.Parse(param[2].ToString());
                        break;
                    }

                    case "SetColor":
                    {
                        SetColor sequence = (this.GetSequence(new SetColor(uneSequence.Nom, 0, 0, 0)) as SetColor);
                        sequence.Rouge = int.Parse(param[0].ToString());
                        sequence.Vert = int.Parse(param[1].ToString());
                        sequence.Bleu = int.Parse(param[2].ToString());
                        break;
                    }

                    case "ChangeBrightness":
                    {
                        ChangeBrightness sequence = (this.GetSequence(new ChangeBrightness(uneSequence.Nom, 0)) as ChangeBrightness);
                        sequence.Intensite = int.Parse(param[0].ToString());
                        break;
                    }

                    case "DrawFromSD":
                    {
                        DrawFromSD sequence = (this.GetSequence(new DrawFromSD(uneSequence.Nom, 0, 0, "")) as DrawFromSD);
                        sequence.PositionX = int.Parse(param[1].ToString());
                        sequence.PositionY = int.Parse(param[2].ToString());
                        sequence.NomFichier = nomFichier;
                        break;
                    }

                    case "SetCache":
                    {
                        SetCache sequence = (this.GetSequence(new SetCache(uneSequence.Nom, 0)) as SetCache);
                        sequence.NumeroCache = int.Parse(param[0].ToString());
                        break;
                    }

                    case "DisplayCache":
                    {
                        DisplayCache sequence = (this.GetSequence(new DisplayCache(uneSequence.Nom, 0)) as DisplayCache);
                        sequence.NumeroCache = int.Parse(param[0].ToString());
                        break;
                    }

                    case "Delay":
                    {
                        Delay sequence = (this.GetSequence(new Delay(uneSequence.Nom, 0)) as Delay);
                        sequence.MilliSecond = int.Parse(param[0].ToString());
                        break;
                    }
                    default: { break; }
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Méthode permettant de retirer une séquence.
        /// </summary>
        /// <param name="uneSequence"></param>
        /// <returns></returns>
        public bool EnleverSequence(Sequence uneSequence)
        {
            if (this.IsSequencePresent(uneSequence))
            {
                this.listeSequence.Remove(uneSequence);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant d'obtenir une séquence dans la liste.
        /// </summary>
        /// <param name="uneSequence"></param>
        /// <returns></returns>
        public Sequence GetSequence(Sequence uneSequence)
        {
            return this.listeSequence.Find(x => x.Equals(uneSequence));
        }

        /// <summary>
        /// Vider la liste de séquence.
        /// </summary>
        public void ViderAllSequence()
        {
            this.listeSequence.Clear();
        }

        /// <summary>
        /// Méthode permettant d'obtenir le nomtre de séquence.
        /// </summary>
        /// <returns></returns>
        public int GetNombreSequence()
        {
            return this.listeSequence.Count;
        }

        /// <summary>
        /// Méthode permettant de trouver une séquence.
        /// </summary>
        /// <param name="uneSequence"></param>
        /// <returns></returns>
        private bool IsSequencePresent(Sequence uneSequence)
        {
            return this.listeSequence.Contains(uneSequence);
        }

        /// <summary>
        /// Méthode d'override ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Nom + " " + this.Boucle;
        }

        /// <summary>
        /// Méthode d'override Equals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return ((this.Nom.Equals((obj as Animation).Nom)));
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

