// /******************************************************
// Projet : AnimaScion
// Auteur(e)(s) : Olivier Lajoie     
// Nom du fichier : ListeSequenceAdapteur.cs
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
    class ListeSequenceAdapteur : BaseAdapter<SequenceDTO>
    {
        private Activity context;
        private IList<SequenceDTO> sequences;

        public ListeSequenceAdapteur(Activity context, IList<SequenceDTO> sequences)
        {
            this.context = context;
            this.sequences = sequences;
        }

        public override SequenceDTO this[int index]
        {
            get { return this.sequences[index]; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.sequences[position].Nom;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this.sequences.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            SequenceDTO item = this.sequences[position];

            View listeSequence = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeSequence, parent, false)) as LinearLayout;

            if (position == 0)
                listeSequence.FindViewById<TextView>(Resource.Id.txtContenuNomSequence).Text = item.Nom;
            else
                listeSequence.FindViewById<TextView>(Resource.Id.txtContenuNomSequence).Text = "Séquence " + (position) + ": " + item.Nom;

            return listeSequence;
        }
    }
}