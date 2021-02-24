﻿using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TheLastTime.Data;
using TheLastTime.Models;

namespace TheLastTime.Components
{
    public sealed partial class CategoryComponent : IDisposable
    {
        public bool editCategory;

        [Inject]
        DataService DataService { get; set; } = null!;

        [Inject]
        State State { get; set; } = null!;

        protected override void OnInitialized()
        {
            DataService.PropertyChanged += PropertyChanged;
            State.PropertyChanged += PropertyChanged;
        }

        void PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            DataService.PropertyChanged -= PropertyChanged;
            State.PropertyChanged -= PropertyChanged;
        }
    }
}
