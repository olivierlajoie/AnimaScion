// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : AnimationActivity.cs
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
using AnimaScion.Adapteur;

using AnimaScion.Vue;

namespace AnimaScion.Vue
{
    /// <summary>
    /// Activité Principale Des Animations.
    /// </summary>
    [Activity(Label = "AnimaScion - Modifier Animation", Icon = "@mipmap/icon")]
    public class AnimationActivity : Activity
    {
        // Attributs.
        private ControleurAnimation monControleur;
        private ListeSequenceAdapteur monAdapteur;

        private IList<SequenceDTO> maListeSequence;

        private string paramNomAnimation;
        private bool paramBoucle;
        private string paramNomSequence;

        private Button buttonAjouter;
        private Button buttonModifier;
        private Button buttonSuprimer;
        private Button buttonFermer;
        private Button buttonAjouterSequence;
        private Button buttonQuitterSequence;
        private Button buttonEnvoyerAnimation;

        private EditText nomAnimation;

        private ToggleButton toggleBoucle;

        private Spinner spinnerSequence;
        private Spinner spinnerAllSequence;

        private View layoutAjouterSequence;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.layoutAjouterSequence = LayoutInflater.Inflate(Resource.Layout.AjouterSequence, null);

            this.SetContentView(Resource.Layout.ModifierAnimation);

            // Initialisation du Controleur.
            this.monControleur = ControleurAnimation.GetInstance();

            this.paramNomAnimation = Intent.GetStringExtra("ParamNom");
            this.paramBoucle = Intent.GetBooleanExtra("ParamBoucle", false);

            this.buttonAjouter = FindViewById<Button>(Resource.Id.buttonAjouter);
            this.buttonModifier = FindViewById<Button>(Resource.Id.buttonModifierAnimation);
            this.buttonSuprimer = FindViewById<Button>(Resource.Id.buttonSuprimerAnimation);
            this.buttonFermer = FindViewById<Button>(Resource.Id.buttonQuitterAnimation);
            this.buttonAjouterSequence = layoutAjouterSequence.FindViewById<Button>(Resource.Id.buttonAjouterSequence);
            this.buttonQuitterSequence = layoutAjouterSequence.FindViewById<Button>(Resource.Id.buttonQuitterSequence);
            this.buttonEnvoyerAnimation = FindViewById<Button>(Resource.Id.buttonEnvoyerAnimation);

            this.spinnerSequence = FindViewById<Spinner>(Resource.Id.spinnerSequence);
            this.spinnerAllSequence = layoutAjouterSequence.FindViewById<Spinner>(Resource.Id.spinnerAllSequence);

            this.nomAnimation = FindViewById<EditText>(Resource.Id.txtNomAnimation);
            this.nomAnimation.Text = this.paramNomAnimation;

            this.toggleBoucle = FindViewById<ToggleButton>(Resource.Id.toggleBoucle);
            this.toggleBoucle.Checked = this.paramBoucle;

            // Actions sur les boutons.
            this.buttonModifier.Click += delegate { ModifierAnimation(); };
            this.buttonSuprimer.Click += delegate { SuprimerAnimation(); };
            this.buttonAjouter.Click += delegate { SetContentView(layoutAjouterSequence); };
            this.buttonAjouterSequence.Click += delegate { AjouterSequence(); };
            this.buttonQuitterSequence.Click += delegate { Finish(); };
            this.buttonFermer.Click += delegate { Finish(); };
            this.buttonEnvoyerAnimation.Click += delegate { monControleur.EnvoyerAnimation(this.nomAnimation.Text); Finish(); };

            // Appels des méthodes des Spinners & Listes.
            RefreshListeSequence();
            RefreshAllSequence();
            SpinnerItemsSelected();
            SpinnerAllItemsSelected();

        }

        /// <summary>
        /// Initialisation d'un OptionsMenu.
        /// </summary>
        /// <param name="menu">Contient les items.</param>
        /// <returns>Menu.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.OptionMenuAnimation, menu);
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
                case Resource.Id.modifieranimation:
                    this.ModifierAnimation();
                    return true;
                case Resource.Id.envoyeranimation:
                    this.monControleur.EnvoyerAnimation(this.paramNomAnimation);
                    return true;
                case Resource.Id.suprimeranimation:
                    this.SuprimerAnimation();
                    return true;
                case Resource.Id.ajoutersequence:
                    this.SetContentView(layoutAjouterSequence);
                    return true;
                case Resource.Id.quitter:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Méthode permettant de Refresh toutes les séquences.
        /// </summary>
        public void RefreshAllSequence()
        {
            var adapteur = ArrayAdapter<String>.CreateFromResource(this, Resource.Array.spinnerSequence, Android.Resource.Layout.SimpleSpinnerItem);
            adapteur.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerAllSequence.Adapter = adapteur;
            spinnerAllSequence.SetSelection(adapteur.Count - 1);
        }

        /// <summary>
        /// Méthode permettant de Refresh la liste des séquences.
        /// </summary>
        public void RefreshListeSequence()
        {
            maListeSequence = monControleur.GetListeSequenceParAnimation(this.nomAnimation.Text);
            monAdapteur = new ListeSequenceAdapteur(this, maListeSequence);
            spinnerSequence.Adapter = monAdapteur;
        }

        /// <summary>
        /// Méthode permettant de déterminer l'itemSelected du Spinner.
        /// </summary>
        public void SpinnerItemsSelected()
        {
            this.spinnerSequence.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                if (e.Position != 0)
                {
                    
                    Intent SequenceDetails = new Intent(this, typeof(SequenceActivity));
                    SequenceDetails.PutExtra("ParamNomAnimation", this.paramNomAnimation);
                    SequenceDetails.PutExtra("ParamNomSequence", this.maListeSequence[e.Position].Nom);
                    StartActivity(SequenceDetails);
                    Finish();
                }
            };
        }

        /// <summary>
        /// Méthode permettant de déterminer l'itemSelected de toutes les séquences du Spinner.
        /// </summary>
        public void SpinnerAllItemsSelected()
        {
            this.spinnerAllSequence.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
            {
                this.paramNomSequence = spinnerAllSequence.GetItemAtPosition(e.Position).ToString();
            };
        }

        /// <summary>
        /// Méthode permettant la supression d'une animation.
        /// </summary>
        public void SuprimerAnimation()
        {
            if (this.monControleur.EnleverAnimation(this.paramNomAnimation))
                Finish();
            else
                AfficherMessage("Application n'exite pas.");
        }

        /// <summary>
        /// Méthode permettant la modification d'une animation.
        /// </summary>
        public void ModifierAnimation()
        {
            if (this.monControleur.ModifierAnimation(this.paramNomAnimation, this.nomAnimation.Text, this.toggleBoucle.Checked))
                Finish();
            else
                AfficherMessage("Application n'exite pas.");
        }

        /// <summary>
        /// Méthode permettant d'ajouter une séquence.
        /// </summary>
        public void AjouterSequence()
        {
            this.layoutAjouterSequence = LayoutInflater.Inflate(Resource.Layout.AjouterSequence, null);
            this.spinnerAllSequence = layoutAjouterSequence.FindViewById<Spinner>(Resource.Id.spinnerAllSequence);
            if (this.monControleur.AjouterSequence(this.paramNomAnimation, this.paramNomSequence))
                Finish();
            else
                AfficherMessage("Vous avez déjà 10 séquences.");
        }

        /// <summary>
        /// Méthode permettant d'afficher un message.
        /// </summary>
        private void AfficherMessage(String message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(message);
            builder.SetPositiveButton("Ok", (sender, args) => { Finish(); });
            builder.Show();
        }
    }
}