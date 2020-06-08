﻿using RedesSociales.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedesSociales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilView : ContentPage
    {
        UsuarioViewModel context = new UsuarioViewModel();
        public PerfilView()
        {
            InitializeComponent();
            BindingContext = context;
        }
    }
}