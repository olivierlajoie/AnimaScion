// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : SequenceActivity.cs
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
using System.Net.Sockets;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using AnimaScion.Controleur;
using AnimaScion.DTO;

namespace AnimaScion.Vue
{
    /// <summary>
    /// Activité Principale Des Séquences.
    /// </summary>
    [Activity(Label = "AnimaScion - Modifier Séquence", Icon = "@mipmap/icon")]
    public class SequenceActivity : Activity
    {
        // Attributs.
        private ControleurAnimation monControleur;

        private string paramNomAnimation;
        private string paramNomSequence;
        private bool paramRotation;

        private byte[] outParam;
        private bool outRotation;
        private string outNomFichier;

        private TextView txtRotation;

        private ToggleButton toggleRotation;
        private ToggleButton toggleFill;

        private EditText nomSequence;
        private EditText param1;
        private EditText param2;
        private EditText param3;
        private EditText param4;
        private EditText param5;
        private EditText nomFichier;

        private Button buttonModifier;
        private Button buttonSuprimer;
        private Button buttonQuitter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.ModifierSequence);

            // Initialisation du Controleur.
            this.monControleur = ControleurAnimation.GetInstance();

            this.paramNomAnimation = Intent.GetStringExtra("ParamNomAnimation");
            this.paramNomSequence = Intent.GetStringExtra("ParamNomSequence");

            this.buttonModifier = FindViewById<Button>(Resource.Id.buttonModifierSequence);
            this.buttonSuprimer = FindViewById<Button>(Resource.Id.buttonSuprimerSequence);
            this.buttonQuitter = FindViewById<Button>(Resource.Id.buttonQuitterSequence);

            this.txtRotation = FindViewById<TextView>(Resource.Id.txtRotation);
            this.toggleRotation = FindViewById<ToggleButton>(Resource.Id.toggleRotation);
            this.toggleFill = FindViewById<ToggleButton>(Resource.Id.toggleFill);
            this.nomSequence = FindViewById<EditText>(Resource.Id.txtNomSequence); 
            this.param1 = FindViewById<EditText>(Resource.Id.txtParam1);
            this.param2 = FindViewById<EditText>(Resource.Id.txtParam2);
            this.param3 = FindViewById<EditText>(Resource.Id.txtParam3);
            this.param4 = FindViewById<EditText>(Resource.Id.txtParam4);
            this.param5 = FindViewById<EditText>(Resource.Id.txtParam5);
            this.nomFichier = FindViewById<EditText>(Resource.Id.txtNomFichier);

            // Initialisation des paramètres propres à chaque séquence.
            this.txtRotation.Visibility = ViewStates.Gone;
            this.toggleRotation.Visibility = ViewStates.Gone;
            this.toggleFill.Visibility = ViewStates.Gone;
            this.param1.Visibility = ViewStates.Gone;
            this.param2.Visibility = ViewStates.Gone;
            this.param3.Visibility = ViewStates.Gone;
            this.param4.Visibility = ViewStates.Gone;
            this.param5.Visibility = ViewStates.Gone;
            this.nomFichier.Visibility = ViewStates.Gone;

            this.nomSequence.Text = this.paramNomSequence;
            this.paramRotation = this.toggleRotation.Checked;
            this.monControleur.GetParamParSequence(this.paramNomAnimation, this.paramNomSequence, out outParam, out outRotation, out outNomFichier);

            this.param1.Text = outParam[0].ToString();
            this.param2.Text = outParam[1].ToString();
            this.param3.Text = outParam[2].ToString();
            this.param4.Text = outParam[3].ToString();
            this.param5.Text = outParam[4].ToString();
            this.toggleRotation.Checked = outRotation;
            this.toggleFill.Checked = outRotation;
            this.nomFichier.Text = outNomFichier.Substring(4);

            // Changement de l'apparence du layout de modification de séquence.
            switch (this.paramNomSequence)
            {
                case "SendBuffer": 
                {
                    this.param1.Visibility = ViewStates.Visible; 
                    this.param2.Visibility = ViewStates.Visible; 
                    this.param3.Visibility = ViewStates.Visible; 
                    this.param4.Visibility = ViewStates.Visible; 
                    
                    this.param1.Hint = "Position X"; 
                    this.param2.Hint = "Position Y"; 
                    this.param3.Hint = "Taille X"; 
                    this.param4.Hint = "Taille Y";
                    
                    break; 
                }

                case "Shift": 
                { 
                    this.txtRotation.Visibility = ViewStates.Visible; 
                    this.toggleRotation.Visibility = ViewStates.Visible; 

                    this.param1.Visibility = ViewStates.Visible; 
                    this.param2.Visibility = ViewStates.Visible; 

                    this.param1.Hint = "Déplacement X"; 
                    this.param2.Hint = "Déplacement Y";
                    
                    break; 
                }

                case "DrawRectangle": 
                { 

                    this.txtRotation.Visibility = ViewStates.Visible; 
                    this.toggleFill.Visibility = ViewStates.Visible;

                    this.txtRotation.Text = "Remplir:";

                    this.param1.Visibility = ViewStates.Visible; 
                    this.param2.Visibility = ViewStates.Visible; 
                    this.param3.Visibility = ViewStates.Visible; 
                    this.param4.Visibility = ViewStates.Visible; 
                    
                    this.param1.Hint = "Position X1"; 
                    this.param2.Hint = "Position X2"; 
                    this.param3.Hint = "Position Y1";
                    this.param4.Hint = "Position Y2";
                    
                    break; 
                }

                case "DrawX": 
                {

                    this.param1.Visibility = ViewStates.Visible;
                    this.param2.Visibility = ViewStates.Visible;
                    this.param3.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Position X";
                    this.param2.Hint = "Position Y";
                    this.param3.Hint = "Taille";

                    break; 
                }

                case "DrawCircle": 
                {

                    this.param1.Visibility = ViewStates.Visible;
                    this.param2.Visibility = ViewStates.Visible;
                    this.param3.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Position X";
                    this.param2.Hint = "Position Y";
                    this.param3.Hint = "Taille";

                    break;
                }

                case "SetColor": 
                {

                    this.param1.Visibility = ViewStates.Visible;
                    this.param2.Visibility = ViewStates.Visible;
                    this.param3.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Rouge";
                    this.param2.Hint = "Vert";
                    this.param3.Hint = "Bleu";

                    break; 
                }

                case "ChangeBrightness": 
                {

                    this.param1.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Intensité";
                    
                    break; 
                }

                case "DrawFromSD":
                {

                    this.nomFichier.Visibility = ViewStates.Visible;
                    this.param2.Visibility = ViewStates.Visible;
                    this.param3.Visibility = ViewStates.Visible;

                    // TEST ou H1 fonctionnent.
                    this.nomFichier.Hint = "Nom Fichier (H1 ou TEST)";
                    this.param2.Hint = "Position X";
                    this.param3.Hint = "Position Y";

                    break; 
                }

                case "SetCache": 
                {

                    this.param1.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Numéro de cache";

                    break;
                }

                case "DisplayCache": 
                {

                    this.param1.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Numéro de cache";

                    break; 
                }

                case "Delay": 
                {

                    this.param1.Visibility = ViewStates.Visible;

                    this.param1.Hint = "Délai (Ms)";

                    break;
                }

                case "Clear":
                {
                    break;
                }

                default: { Finish();  break; }
            }

            // Actions sur les boutons.
            this.buttonModifier.Click += delegate { ModifierSequence(); };
            this.buttonSuprimer.Click += delegate { SuprimerSequence(); };
            this.buttonQuitter.Click += delegate { Finish(); };

        }

        /// <summary>
        /// Initialisation d'un OptionsMenu.
        /// </summary>
        /// <param name="menu">Contient les items.</param>
        /// <returns>Menu.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.OptionMenuSequence, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// Initialisation des items de l'OptionsMenu.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.modifiersequence:
                    this.ModifierSequence();
                    return true;
                case Resource.Id.suprimersequence:
                    this.SuprimerSequence();
                    return true;
                case Resource.Id.quitter:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Méthode permettant la modification d'une séquence.
        /// </summary>
        public void ModifierSequence()
        {
            byte[] param = { byte.Parse(param1.Text), byte.Parse(param2.Text), byte.Parse(param3.Text), byte.Parse(param4.Text), byte.Parse(param5.Text) };
            this.monControleur.ModifierSequence(paramNomAnimation, paramNomSequence, param, (paramNomSequence == "Shift") ? toggleRotation.Checked : toggleFill.Checked, nomFichier.Text);
            Finish();
        }

        /// <summary>
        /// Méthode permettant la supression d'une séquence.
        /// </summary>
        public void SuprimerSequence()
        {
            this.monControleur.EnleverSequence(this.paramNomAnimation, this.paramNomSequence);
            Finish();
        }
    }
}