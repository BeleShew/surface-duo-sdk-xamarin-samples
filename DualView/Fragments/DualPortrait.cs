﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;

namespace DualView.Fragments
{
	public class DualPortrait : BaseFragment, ItemsListFragment.IOnItemSelectedListener
	{
		int currentSelectedPosition;
		ItemsListFragment itemListFragment;
		List<Item> items;

		public static DualPortrait NewInstance(List<Item> items)
		{
			DualPortrait dualPortrait = new DualPortrait();
			dualPortrait.items = items;
			return dualPortrait;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			itemListFragment = ItemsListFragment.NewInstance(items);
			itemListFragment.RegisterOnItemSelectedListener(this);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.fragment_dual_portrail, container, false);
			ShowFragment(itemListFragment, Resource.Id.master_dual);
			return view;
		}

		public override void OnHiddenChanged(bool hidden)
		{
			base.OnHiddenChanged(hidden);

			if (!hidden)
			{
				ShowBackOnActionBar();
				itemListFragment.SetSelectedItem(currentSelectedPosition);
			}
		}

		void ShowFragment(Fragment fragment, int id)
		{
			var fragmentTransaction = ChildFragmentManager.BeginTransaction();
			var showFragment = ChildFragmentManager.FindFragmentById(id);
			if (showFragment == null)
				fragmentTransaction.Add(id, fragment);
			else
				fragmentTransaction.Remove(showFragment).Add(id, fragment);
			fragmentTransaction.Commit();
		}

		public void OnItemSelected(Item item, int position)
		{
			if (listener != null)
				listener.OnItemSelected(position);
			currentSelectedPosition = position;
			// Showing ItemDetailFragment on the right screen when the app is in spanned mode
			ShowFragment(ItemDetailFragment.NewInstance(item), Resource.Id.master_detail);
		}

		void ShowBackOnActionBar()
		{
			if (Activity != null)
			{
				var actionbar = ((AppCompatActivity)Activity).SupportActionBar;
				if (actionbar != null)
				{
					actionbar.SetDisplayHomeAsUpEnabled(false);
					actionbar.SetHomeButtonEnabled(false);
				}
			}
		}

		public override bool OnBackPressed()
			=> false;

		public override void SetCurrentSelectedPosition(int position)
		{
			currentSelectedPosition = position;
		}
	}
}