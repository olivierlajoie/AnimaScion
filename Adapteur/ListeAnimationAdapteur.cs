// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : ListeAnimationAdapteur.cs
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

using AnimaScion.DTO;

namespace AnimaScion.Adapteur
{
    class ListeAnimationAdapteur : BaseAdapter<AnimationDTO>
    {
        private Activity context;
		private IList<AnimationDTO> animations;

		public ListeAnimationAdapteur (Activity context, IList<AnimationDTO> animations)
		{
			this.context = context;
			this.animations = animations;
		}

		public override AnimationDTO this[int index]
		{
			get { return this.animations[index]; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count
		{
			get { return this.animations.Count; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

            AnimationDTO item = this.animations[position];

            View listeAnimation = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAnimation, parent, false)) as LinearLayout;

            listeAnimation.FindViewById<TextView>(Resource.Id.txtContenuNomAnimation).Text = item.Nom;
            if (item.Boucle == true)
                listeAnimation.FindViewById<TextView>(Resource.Id.txtContenuBoucle).Text = "Oui";
            else
                listeAnimation.FindViewById<TextView>(Resource.Id.txtContenuBoucle).Text = "Non";
            listeAnimation.FindViewById<TextView>(Resource.Id.txtContenuNbrSequence).Text = (item.NombreSequence - 1).ToString();

			return listeAnimation;
        }
    }
}