// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : ControleurAnimaScion.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Android.App;

using AnimaScion.Modele;
using AnimaScion.DTO;

namespace AnimaScion.Controleur
{
    /// <summary>
    /// Classe du Controleur
    /// </summary>
	public class ControleurAnimation
	{
		private Animateur monAnimateur;
		private static ControleurAnimation instance;
        private int tour = 0;

        /// <summary>
        /// Constructeur par defaut.
        /// </summary>
		public ControleurAnimation()
		{
			monAnimateur = new Animateur();
            //if (File.Exists(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Android/data/AnimaScion.AnimaScion/files/Animations.xml"))
				//ChargerAnimation();
		}

        /// <summary>
        /// Méthode Singleton.
        /// </summary>
        /// <returns></returns>
		public static ControleurAnimation GetInstance()
		{
			if (instance == null)
				instance = new ControleurAnimation();
			return instance;
		}

        ///// <summary>
        ///// Méthode de sérialization XML.
        ///// </summary>
        //public void SauvegarderAnimation()
        //{
        //    if (!Directory.Exists(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Android/data/AnimaScion.AnimaScion/files/"))
        //        Directory.CreateDirectory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Android/data/AnimaScion.AnimaScion/files/");
        //    File.Delete(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Android/data/AnimaScion.AnimaScion/files/Animations.xml");
        //    FileStream stream = File.OpenWrite(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Android/data/AnimaScion.AnimaScion/files/Animations.xml");
        //    XmlSerializer monSerializer = new XmlSerializer(typeof(Animateur));
        //    monSerializer.Serialize(stream, this.monAnimateur);
        //    stream.Close();
        //}

        ///// <summary>
        ///// Méthode de désérialization XML.
        ///// </summary>
        //public void ChargerAnimation()
        //{
        //    XmlSerializer monSerializer = new XmlSerializer(typeof(Animateur));
        //    FileStream stream = File.OpenRead(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Android/data/AnimaScion.AnimaScion/files/Animations.xml");
        //    this.monAnimateur = (Animateur)monSerializer.Deserialize(stream);
        //    stream.Close();
        //}

        /// <summary>
        /// Méthode d'envoie au MC ESP.
        /// </summary>
        /// <param name="nom"></param>
        public void EnvoyerAnimation(string nom)
        {
            try
            {
                // Connexion.
                UdpClient monClientUdp = new UdpClient();
                monClientUdp.Connect("192.168.2.18", 8818);

                Animation uneAnimation = new Animation(nom);
                Animation monAnimation = monAnimateur.GetAnimation(uneAnimation);
                List<Sequence> maListeSequence = new List<Sequence>(monAnimateur.GetAnimation(uneAnimation).GetAllSequence());

                // Construction des buffers.
                foreach (var sequence in maListeSequence)
                {

                    if (sequence.Nom == "Clear")
                    {
                        SetColor color = new SetColor("SetColor", 0, 0, 0);
                        DrawRectangle clear = new DrawRectangle("DrawRectangle", 0, 16, 0, 16, true);
                        DrawRectangle clear2 = new DrawRectangle("DrawRectangle", 16, 31, 0, 16, true);

                        byte[] buffer = color.Afficher();
                        byte[] buffer1 = clear.Afficher();
                        byte[] buffer2 = clear2.Afficher();

                        monClientUdp.Send(buffer, buffer.Length);
                        Thread.Sleep(1000);
                        monClientUdp.Send(buffer1, buffer1.Length);
                        Thread.Sleep(1000);
                        monClientUdp.Send(buffer2, buffer2.Length);
                        Thread.Sleep(1000);
                    }
                    else if (sequence.Nom != "Choisir Séquence")
                    {
                        byte[] buffer = sequence.Afficher();

                        monClientUdp.Send(buffer, buffer.Length);
                        Thread.Sleep(1000);
                    }
                }

                // Animation en boucle. (2 max)
                if (monAnimation.Boucle == true)
                {
                    while (tour <= 2)
                    {
                        tour++;
                        EnvoyerAnimation(nom);
                    }
                }
                monClientUdp.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur: " + e.Message);
            }
        }

        /// <summary>
        /// Méthode permettant d'obtenir la liste des animations d'un animateur.
        /// </summary>
        /// <returns></returns>
        public List<AnimationDTO> GetListeAnimations()
        {
            List<AnimationDTO> listeAnimations = new List<AnimationDTO>();
            foreach (var animation in this.monAnimateur.GetAllAnimation())
            {
                AnimationDTO uneAnimation = new AnimationDTO(animation);
                listeAnimations.Add(uneAnimation);
            }
            return listeAnimations;
        }

        /// <summary>
        /// Méthode permettant d'obtenir la liste des séquences par animation.
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public List<SequenceDTO> GetListeSequenceParAnimation(string nom)
        {
            Animation uneAnimation = new Animation(nom);
            List<SequenceDTO> listeSequences = new List<SequenceDTO>();
            Animation animation = this.monAnimateur.GetAnimation(uneAnimation);

            if (animation != null)
            {
                foreach (var sequence in this.monAnimateur.GetAnimation(uneAnimation).GetAllSequence())
                {
                    if (this.monAnimateur.GetAnimation(uneAnimation).GetAllSequence().Count != 0)
                    {
                        SequenceDTO uneSequence = new SequenceDTO(sequence);
                        listeSequences.Add(uneSequence);
                    }
                }
            }
            return listeSequences;
        }

        /// <summary>
        /// Méthode permettant d'ajouter une animation.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="boucle"></param>
        /// <returns></returns>
        public bool AjouterAnimation(string nom, bool boucle)
        {
            Animation animation = new Animation(nom);
            return this.monAnimateur.AjouterAnimation(animation, boucle);
        }

        /// <summary>
        /// Méthode permettant de modifier une animation.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="changementNom"></param>
        /// <param name="changementBoucle"></param>
        /// <returns></returns>
        public bool ModifierAnimation(string nom, string changementNom, bool changementBoucle)
        {
            Animation uneAnimation = new Animation(nom);
            return this.monAnimateur.ModifierAnimation(uneAnimation, changementNom, changementBoucle);
        }

        /// <summary>
        /// Méthode permettant de retirer une animation.
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public bool EnleverAnimation(string nom)
        {
            Animation animation = new Animation(nom);
            return this.monAnimateur.EnleverAnimation(animation);
        }

        /// <summary>
        /// Méthode permettant d'ajouter une séquence.
        /// </summary>
        /// <param name="nomAnimation"></param>
        /// <param name="nomSequence"></param>
        /// <returns></returns>
        public bool AjouterSequence(string nomAnimation, string nomSequence)
        {
            switch (nomSequence)
            {
                case "SendBuffer": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new SendBuffer(nomSequence, 0, 0, 16, 16, new byte[] { 100, 0, 0, 0, 100, 0, 0, 0, 100 })); }
                case "Shift": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new Shift(nomSequence, 2, 3, false)); }
                case "DrawRectangle": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new DrawRectangle(nomSequence, 5, 10, 5, 10, false)); }
                case "DrawX": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new DrawX(nomSequence, 5, 5, 5)); }
                case "DrawCircle": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new DrawCircle(nomSequence, 10, 2, 2)); }
                case "SetColor": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new SetColor(nomSequence, 5, 5, 5)); }
                case "ChangeBrightness": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new ChangeBrightness(nomSequence, 10)); }
                case "DrawFromSD": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new DrawFromSD(nomSequence, 0, 0, "H1")); }
                case "SetCache": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new SetCache(nomSequence, 0)); }
                case "DisplayCache": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new DisplayCache(nomSequence, 0)); }
                case "Delay": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new Delay(nomSequence, 5)); }
                case "Clear": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).AjouterSequence(new DrawRectangle("Clear", 0, 16, 0, 16, false)); }
                default: { return false; }
            }
        }

        /// <summary>
        /// Méthode permettant de modifier une séquence.
        /// </summary>
        /// <param name="nomAnimation"></param>
        /// <param name="nomSequence"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="selection"></param>
        /// <param name="nomFichier"></param>
        /// <returns></returns>
        public bool ModifierSequence(string nomAnimation, string nomSequence, byte[] param, bool selection, string nomFichier)
        {
            return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).ModifierSequence(nomSequence, param, selection, nomFichier); 
        }

        /// <summary>
        /// Méthode permettant de retirer une séquence.
        /// </summary>
        /// <param name="nomAnimation"></param>
        /// <param name="nomSequence"></param>
        /// <returns></returns>
        public bool EnleverSequence(string nomAnimation, string nomSequence)
        {
            switch (nomSequence)
            {
                case "SendBuffer": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new SendBuffer(nomSequence, 0, 0, 0, 0, null)); }
                case "Shift": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new Shift(nomSequence, 0, 0, false)); }
                case "DrawRectangle": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new DrawRectangle(nomSequence, 0, 0, 0, 0, false)); }
                case "DrawX": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new DrawX(nomSequence, 0, 0, 0)); }
                case "DrawCircle": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new DrawCircle(nomSequence, 0, 0, 0)); }
                case "SetColor": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new SetColor(nomSequence, 0, 0, 0)); }
                case "ChangeBrightness": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new ChangeBrightness(nomSequence, 0)); }
                case "DrawFromSD": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new DrawFromSD(nomSequence, 0, 0, "")); }
                case "SetCache": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new SetCache(nomSequence, 0)); }
                case "DisplayCache": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new DisplayCache(nomSequence, 0)); }
                case "Delay": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new Delay(nomSequence, 0)); }
                case "Clear": { return this.monAnimateur.GetAnimation(new Animation(nomAnimation)).EnleverSequence(new DrawRectangle(nomSequence, 0, 0, 0, 0, true)); }
                default: { return false; }
            }
        }

        public SequenceDTO GetParamSequence(string nomAnimation, string nomSequence)
        {
            SendBuffer uneSequence = new SendBuffer(nomSequence, 0, 0, 0, 0, null);
            return new SequenceDTO(this.monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(uneSequence));
        }


        /// <summary>
        /// Méthode permettant de retourner les paramètres spécifiques à une séquence.
        /// </summary>
        /// <param name="nomAnimation"></param>
        /// <param name="nomSequence"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="rotation"></param>
        /// <param name="nomFichier"></param>
        public void GetParamParSequence(string nomAnimation, string nomSequence, out byte[] param, out bool rotation, out string nomFichier)
        {
            param = new byte[5];
            param[0] = 0;
            param[1] = 0;
            param[2] = 0;
            param[3] = 0;
            param[4] = 0; 
            rotation = false;
            nomFichier = "";

            switch(nomSequence)
            {
                case "SendBuffer":
                {
                    SendBuffer sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new SendBuffer(nomSequence, 0, 0, 0, 0, null)) as SendBuffer);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.PositionX.ToString());
                        param[1] = byte.Parse(sequence.PositionY.ToString());
                        param[2] = byte.Parse(sequence.TailleX.ToString());
                        param[3] = byte.Parse(sequence.TailleY.ToString());
                    }
                    break;
                }

                case "Shift":
                {

                    Shift sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new Shift(nomSequence, 0, 0, false)) as Shift);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.PositionX.ToString()); ;
                        param[1] = byte.Parse(sequence.PositionY.ToString()); ;
                        rotation = sequence.Rotation;
                    }
                    break;
                }

                case "DrawRectangle":
                {
                    DrawRectangle sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new DrawRectangle(nomSequence, 0, 0, 0, 0, false)) as DrawRectangle);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.PositionX.ToString());
                        param[1] = byte.Parse(sequence.PositionY.ToString());
                        param[2] = byte.Parse(sequence.PositionY1.ToString());
                        param[3] = byte.Parse(sequence.PositionY2.ToString());
                        rotation = sequence.Fill;
                    }
                    break;
                }

                case "DrawX":
                {
                    DrawX sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new DrawX(nomSequence, 0, 0, 0)) as DrawX);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.PositionX.ToString());
                        param[1] = byte.Parse(sequence.PositionY.ToString());
                        param[2] = byte.Parse(sequence.Size.ToString());
                    }
                    break;
                }

                case "DrawCircle":
                {
                    DrawCircle sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new DrawCircle(nomSequence, 0, 0, 0)) as DrawCircle);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.PositionX.ToString());
                        param[1] = byte.Parse(sequence.PositionY.ToString());
                        param[2] = byte.Parse(sequence.Size.ToString());
                    }
                    break;
                }

                case "SetColor":
                {
                    SetColor sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new SetColor(nomSequence, 0, 0, 0)) as SetColor);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.Rouge.ToString());
                        param[1] = byte.Parse(sequence.Vert.ToString());
                        param[2] = byte.Parse(sequence.Bleu.ToString());
                    }
                    break;
                }

                case "ChangeBrightness":
                {
                    ChangeBrightness sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new ChangeBrightness(nomSequence, 0)) as ChangeBrightness);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.Intensite.ToString());
                    }
                    break;
                }

                case "DrawFromSD":
                {
                    DrawFromSD sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new DrawFromSD(nomSequence, 0, 0, "")) as DrawFromSD);
                    if (sequence != null)
                    {
                        param[1] = byte.Parse(sequence.PositionX.ToString());
                        param[2] = byte.Parse(sequence.PositionY.ToString());
                        nomFichier = sequence.NomFichier;
                    }
                    break;
                }

                case "SetCache":
                {
                    SetCache sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new SetCache(nomSequence, 0)) as SetCache);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.NumeroCache.ToString()); 
                    }
                    break;
                }

                case "DisplayCache":
                {
                    DisplayCache sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new DisplayCache(nomSequence, 0)) as DisplayCache);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.NumeroCache.ToString()); 
                    }
                    break;
                }

                case "Delay":
                {
                    Delay sequence = (monAnimateur.GetAnimation(new Animation(nomAnimation)).GetSequence(new Delay(nomSequence, 0)) as Delay);
                    if (sequence != null)
                    {
                        param[0] = byte.Parse(sequence.MilliSecond.ToString()); 
                    }
                    break;
                }

                default: { break; }
            }
        }
	}
}

