﻿using Blazor.IndexedDB.Framework;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastTime.Data;

namespace TheLastTime.Pages
{
    public partial class Index
    {
        protected Category selectedCategory = new Category();

        protected Habit? selectedHabit;

        List<Category> categoryList = new List<Category>();
        List<Habit> habitList = new List<Habit>();
        List<Time> timeList = new List<Time>();

        Dictionary<long, Category> categoryDict = new Dictionary<long, Category>();
        Dictionary<long, Habit> habitDict = new Dictionary<long, Habit>();
        //Dictionary<long, Time> timeDict = new Dictionary<long, Time>();

        bool editCategory;
        bool editHabit;
        bool editTime;

        [Inject]
        IIndexedDbFactory DbFactory { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            using IndexedDatabase db = await DbFactory.Create<IndexedDatabase>();

            categoryList = db.Categories.ToList();
            habitList = db.Habits.ToList();
            timeList = db.Times.ToList();

            categoryDict = categoryList.ToDictionary(category => category.Id);
            habitDict = habitList.ToDictionary(habit => habit.Id);
            //timeDict = timeList.ToDictionary(time => time.Id);

            foreach (Time time in timeList)
            {
                if (habitDict.ContainsKey(time.HabitId))
                    habitDict[time.HabitId].TimeList.Add(time);
            }

            foreach (Habit habit in habitList)
            {
                if (categoryDict.ContainsKey(habit.CategoryId))
                    categoryDict[habit.CategoryId].HabitList.Add(habit);
            }
        }

        async Task SaveCategory(Category category)
        {
            using IndexedDatabase db = await this.DbFactory.Create<IndexedDatabase>();

            if (category.Id == 0)
            {
                db.Categories.Add(category);
            }

            await db.SaveChanges();

            await OnInitializedAsync();
        }

        async Task DeleteCategory(Category category)
        {
            using IndexedDatabase db = await this.DbFactory.Create<IndexedDatabase>();
            db.Categories.Remove(category);
            await db.SaveChanges();

            await OnInitializedAsync();
        }

        async Task SaveHabit(Habit habit)
        {
            using IndexedDatabase db = await this.DbFactory.Create<IndexedDatabase>();

            if (habit.Id == 0)
            {
                db.Habits.Add(habit);
            }

            await db.SaveChanges();

            await OnInitializedAsync();
        }

        async Task DeleteHabit(Habit habit)
        {
            using IndexedDatabase db = await this.DbFactory.Create<IndexedDatabase>();
            db.Habits.Remove(habit);
            await db.SaveChanges();

            await OnInitializedAsync();
        }

        async Task DeleteTime(Time time)
        {
            using IndexedDatabase db = await this.DbFactory.Create<IndexedDatabase>();
            db.Times.Remove(time);
            await db.SaveChanges();

            await OnInitializedAsync();
        }

        async Task AddTime(Habit habit)
        {
            using IndexedDatabase db = await this.DbFactory.Create<IndexedDatabase>();

            Time time = new Time
            {
                HabitId = habit.Id,
                DateTime = DateTime.Now
            };

            db.Times.Add(time);

            await db.SaveChanges();

            await OnInitializedAsync();
        }
    }
}
