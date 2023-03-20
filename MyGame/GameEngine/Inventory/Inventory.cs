﻿using GameEngine;
using SFML.System;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace MyGame.GameEngine.Inventory
{
    internal class Inventory : GameObject
    {
        private Vector2f scale = new Vector2f(4, 4);
        private const int sizex = 9;
        private const int sizey = 4;
        private ItemSlot[] slots;
        public Inventory(Scene scene)
        {
            slots = new ItemSlot[sizex * sizey];
            for(int i = 0; i < sizex * sizey; i++)
            {
                slots[i] = new ItemSlot(this, i, scale, new Vector2f((i % sizex) * scale.X * 20, (i / sizex) * scale.Y * 20));
                slots[i].SetItem(new Item(Game.Random.Next(4) - 1, Game.Random.Next(80) + 1));
                scene.AddUiElement(slots[i]);
            }
        }
        public override void Draw() { }
        public override void Update(Time elapsed)
        {
            if (Game._Mouse.IsLeftJustReleased())
            {
                //making a list of all slots that were selected
                List<ItemSlot> selected = new List<ItemSlot>() { };
                foreach (ItemSlot slot in slots) 
                {
                    if (slot.selected) { selected.Add(slot); slot.Deselect(); }
                }

                //if only one slot was selected it does one function but if multiple were it calls a different one
                if(selected.Count == 1) { SlotClicked(selected[0].ID); }
                else { SlotsClicked(selected); }
            }
            if(Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                Sort();
            }
        }
        public void SlotsClicked(List<ItemSlot> selected)//when more than one slot is selected
        {
            if (Game._Mouse.item.ID != -1)
            {
                //remove things that cant have things put in them
                Predicate<ItemSlot> removeStuff = g => (g._item.ID != Game._Mouse.item.ID && g._item.ID != -1);
                selected.RemoveAll(removeStuff);

                //stop going if there are no slots left to put anything in
                if (selected.Count == 0) { return; }

                //tries to put items in slots evenly and leaves the rest in the mouse cursor
                int itemsPerSlot = Game._Mouse.item.amount / selected.Count;
                int remainingItems = Game._Mouse.item.amount % selected.Count;
                foreach(ItemSlot slot in selected)
                {
                    if(slot._item.amount + itemsPerSlot >= ItemDat.GetStackSize(Game._Mouse.item.ID)) { remainingItems += (slot._item.amount + itemsPerSlot - ItemDat.GetStackSize(Game._Mouse.item.ID)); slot.AddItem(new Item(Game._Mouse.item.ID, itemsPerSlot)); }
                    else { slot.AddItem(new Item(Game._Mouse.item.ID, itemsPerSlot)); }
                }
                //for (int i = 0; i < trash.Count; i++) { selected.Remove(trash[i]); }
                Game._Mouse.SetItem(new Item(Game._Mouse.item.ID, remainingItems));
            }
        }
        public void SlotClicked(int ID) //for when one slot is clicked
        {
            //swaps slots if they are dissimilar
            if (slots[ID]._item.ID != Game._Mouse.item.ID)
            {
                Item temp = slots[ID]._item;
                slots[ID].SetItem(Game._Mouse.item);
                Game._Mouse.SetItem(temp);
            }
            //merges slots into 1 if they fit
            else if (ItemDat.GetStackSize(slots[ID]._item.ID) >= slots[ID]._item.amount + Game._Mouse.item.amount)
            {
                slots[ID].SetItem(new Item(slots[ID]._item.ID, slots[ID]._item.amount + Game._Mouse.item.amount));
                Game._Mouse.SetItem(new Item(-1, 0));
            }
            //fills up slot an puts rest in mouse if they don't fit in one slot
            else
            {
                Game._Mouse.SetItem(new Item(Game._Mouse.item.ID, (Game._Mouse.item.amount + slots[ID]._item.amount) % ItemDat.GetStackSize(Game._Mouse.item.ID)));
                slots[ID].SetItem(new Item(Game._Mouse.item.ID, ItemDat.GetStackSize(Game._Mouse.item.ID)));
            }
        }
        public void Sort()
        {
            int[] items = new int[ItemDat.itemCount];

            //counts up how many of each item there are and clears the inventory
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i]._item.ID >= 0 && slots[i]._item.ID < ItemDat.itemCount)
                {
                    items[slots[i]._item.ID] += slots[i]._item.amount;
                }
                slots[i].SetItem(new Item(-1,0));
            }

            //goes through every item type in order and adds it back to the inventory
            int currentSlot = 0;
            for(int i = 0; i < items.Length; i++)
            {
                while (items[i] > 0)
                {
                    slots[currentSlot].SetItem(new Item(i, items[i]));
                    items[i] -= ItemDat.GetStackSize(i);
                    currentSlot++;
                }
            }
        }
    }
}
