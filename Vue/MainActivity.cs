// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : MainActivity.cs
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

using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Security.Permissions;

using Android.Widget;
using Android.OS;
using Android.Views;
using Android.App;
using Android.Content;

using AnimaScion.Controleur;
using AnimaScion.DTO;
using AnimaScion.Adapteur;

namespace AnimaScion.Vue
{
    /// <summary>
    /// Activité Principale.
    /// </summary>
	[Activity (Label = "AnimaScion", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
        // Attributs.
        private ControleurAnimation monControleur;

        private IList<AnimationDTO> maListeAnimation;

        private ListeAnimationAdapteur monAdapteur;

        private EditText txtNomAnimation;
        private ToggleButton togBoucleAnimation;

        private TextView nombreAnimation;

        private Button buttonAjouter;
        private Button buttonQuitter;
        private Button buttonAjouterAnimation;
        private Button buttonQuitterAnimation;
        private Button buttonInfo;

        private ListView maGrilleListeAnimation;

        private View layoutAnimation;
        private View layoutMain;
        private View layoutInfo;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

            this.layoutMain = LayoutInflater.Inflate(Resource.Layout.Main, null);

            this.SetContentView(layoutMain);

            // Initialisation du Controleur.
            this.monControleur = ControleurAnimation.GetInstance();

            this.maGrilleListeAnimation = layoutMain.FindViewById<ListView>(Resource.Id.listAnimation);
            this.nombreAnimation = layoutMain.FindViewById<TextView>(Resource.Id.txtNbrAnimation);
            this.buttonAjouterAnimation = layoutMain.FindViewById<Button>(Resource.Id.buttonAjouterAnimation);
            this.txtNomAnimation = layoutMain.FindViewById<EditText>(Resource.Id.txtNomAnimation);
            this.togBoucleAnimation = layoutMain.FindViewById<ToggleButton>(Resource.Id.toggleBoucle);
            this.buttonQuitterAnimation = layoutMain.FindViewById<Button>(Resource.Id.bouttonQuitterAnimation);
            this.buttonAjouter = layoutMain.FindViewById<Button>(Resource.Id.buttonAjouter);
            this.buttonQuitter = layoutMain.FindViewById<Button>(Resource.Id.buttonQuitter);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Nosensor;

            this.ButtonClickMain();
		}

        /// <summary>
        /// Override OnResume pour Refresh ma Liste.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            this.RefreshListeAnimation();
        }

        protected override void OnStop()
        {
            base.OnStop();
            //this.monControleur.SauvegarderAnimation();
        }

        /// <summary>
        /// Méthode permettant de Refresh la liste d'animations.
        /// </summary>
        public void RefreshListeAnimation()
        {
            this.maListeAnimation = monControleur.GetListeAnimations();
            this.monAdapteur = new ListeAnimationAdapteur(this, this.maListeAnimation);
            this.maGrilleListeAnimation.Adapter = monAdapteur;
            this.nombreAnimation.Text = monControleur.GetListeAnimations().Count.ToString() + " Animations";
        }

        /// <summary>
        /// Méthode permettant d'appeler l'activité de modification des animations.
        /// </summary>
        public void ButtonClickMain()
        {
            this.maGrilleListeAnimation.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                Intent AnimationDetails = new Intent(this, typeof(AnimationActivity));
                AnimationDetails.PutExtra("ParamNom", this.maListeAnimation[e.Position].Nom);
                AnimationDetails.PutExtra("ParamBoucle", this.maListeAnimation[e.Position].Boucle);
                StartActivity(AnimationDetails);
            };

            // Actions sur les boutons.
            this.buttonAjouter.Click += delegate { this.AjouterAnimation(); };
            this.buttonQuitter.Click += delegate { Finish(); };
        }

        /// <summary>
        /// Initialisation d'un OptionsMenu.
        /// </summary>
        /// <param name="menu">Contient les items.</param>
        /// <returns>Menu.</returns>
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			this.MenuInflater.Inflate(Resource.Menu.OptionMenuMain, menu);
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
			case Resource.Id.nouvelleanimation:
				this.AjouterAnimation();
				return true;
			case Resource.Id.info:
				this.Infos();
				return true;
			case Resource.Id.quitter:
                Finish();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}

        /// <summary>
        /// Méthode permettant d'ajouter une animation.
        /// </summary>
        public void AjouterAnimation()
        {
            this.layoutAnimation = LayoutInflater.Inflate(Resource.Layout.AjouterAnimation, null);
            this.layoutMain = LayoutInflater.Inflate(Resource.Layout.Main, null);

            this.SetContentView(layoutAnimation);

            this.maGrilleListeAnimation = layoutMain.FindViewById<ListView>(Resource.Id.listAnimation);
            this.nombreAnimation = layoutMain.FindViewById<TextView>(Resource.Id.txtNbrAnimation);
            this.buttonAjouter = layoutMain.FindViewById<Button>(Resource.Id.buttonAjouter);
            this.buttonQuitter = layoutMain.FindViewById<Button>(Resource.Id.buttonQuitter);

            this.buttonAjouterAnimation = layoutAnimation.FindViewById<Button>(Resource.Id.buttonAjouterAnimation);
            this.txtNomAnimation = layoutAnimation.FindViewById<EditText>(Resource.Id.txtNomAnimation);
            this.togBoucleAnimation = layoutAnimation.FindViewById<ToggleButton>(Resource.Id.toggleBoucle);
            this.buttonQuitterAnimation = layoutAnimation.FindViewById<Button>(Resource.Id.bouttonQuitterAnimation);

            this.buttonAjouterAnimation.Click += delegate
            {
                if (this.IsEntreesValides())
                {
                    if (this.monControleur.AjouterAnimation(txtNomAnimation.Text, togBoucleAnimation.Checked))
                    {
                        this.txtNomAnimation.Text = "";
                        this.RefreshListeAnimation();
                        this.SetContentView(layoutMain);
                    }
                    else
                        this.AfficherMessage("Animation existante déjà.");
                }
                else
                    this.AfficherMessage("Veuillez remplir tous les champs.");
            };

            this.buttonQuitterAnimation.Click += delegate { this.RefreshListeAnimation(); this.SetContentView(layoutMain); };

            this.ButtonClickMain();
        }

        /// <summary>
        /// Méthode permettant d'afficher les informations de l'application.
        /// </summary>
        public void Infos()
        {
            this.layoutInfo = LayoutInflater.Inflate(Resource.Layout.Info, null);
            this.layoutMain = LayoutInflater.Inflate(Resource.Layout.Main, null);

            this.SetContentView(layoutInfo);

            this.maGrilleListeAnimation = layoutMain.FindViewById<ListView>(Resource.Id.listAnimation);
            this.nombreAnimation = layoutMain.FindViewById<TextView>(Resource.Id.txtNbrAnimation);
            this.buttonAjouter = layoutMain.FindViewById<Button>(Resource.Id.buttonAjouter);
            this.buttonQuitter = layoutMain.FindViewById<Button>(Resource.Id.buttonQuitter);

            this.buttonInfo = layoutInfo.FindViewById<Button>(Resource.Id.buttonInfo);
            this.buttonInfo.Click += delegate { RefreshListeAnimation(); SetContentView(layoutMain); };

            this.ButtonClickMain();
        }

        /// <summary>
        /// Méthode pour afficher des messages.
        /// </summary>
        /// <param name="message">Le Message.</param>
        private void AfficherMessage(String message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Erreur");
            builder.SetMessage(message);
            builder.SetPositiveButton("Ok", (sender, args) => { });
            builder.Show();
        }

        /// <summary>
        /// Méthode permettant de déterminer si la nom d'une animation est valide.
        /// </summary>
        /// <returns></returns>
        public bool IsEntreesValides()
        {
            return ((txtNomAnimation.Text.Length > 0));
        }
	}
}


