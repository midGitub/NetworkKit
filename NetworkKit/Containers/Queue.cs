﻿// MIT License
//
// Copyright (c) 2018 Rodrigo Martins 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Author:
//    Rodrigo Martins <rodrigo.martins.071090@gmail.com>
//

using NetworkKit.System;


namespace Containers {
	/// <summary>
	/// Asynchronous queue
	/// </summary>
	public class Queue<ITEM> {
		/// <summary>Node</summary>
		private class Node {
			/// <summary>Item</summary>
			public ITEM Item;

			/// <summary>Next</summary>
			public Node Next;
		};


		/// <summary>Sync lock</summary>
		private readonly object SyncLock = new object();


		/// <summary>Head queue</summary>
		private Node Head;

		/// <summary>Tail queue</summary>
		private Node Tail;

		/// <summary>Number of items</summary>
		private int Items;


		/// <summary>Is empty</summary>
		public virtual bool IsEmpty {
			get {return (Items == 0);}
		}

		/// <summary>Count</summary>
		public virtual int Count {
			get {return Items;}
		}


		/// <summary>
		/// Asynchronous queue
		/// </summary>
		public Queue(){
			Head = null;
			Tail = null;
		}


		/// <summary>Clear the queue</summary>
		public virtual void Clear(){
			lock(SyncLock){
				Head = null;
				Tail = null;

				Items = 0;
			}
		}

		/// <summary>Queue</summary>
		/// <param name="item">Item <typeparamref name="ITEM"/></param>
		public void Enqueue(ITEM item){
			if((object)item == null) throw new ArgumentNullException(nameof(item));

			lock(SyncLock){
				var node  = new Node();
				node.Item = item;

				// If the head is empty
				if(Head == null) Head = node;

				// Não está vazio
				else Tail.Next = node;

				// Coloque na cauda
				Tail = node;

				Items++;
			}
		}

		/// <summary>Dequeue</summary>
		/// <returns>Item <typeparamref name="ITEM"/></returns>
		public ITEM Dequeue(){
			ITEM item;
			TryDequeue(out item);
			return item;
		}

		/// <summary>Get an item without removing</summary>
		/// <returns>Item <typeparamref name="ITEM"/></returns>
		public ITEM Peek(){
			ITEM item;
			TryPeek(out item);
			return item;
		}

		/// <summary>Try dequeue</summary>
		/// <param name="item">Item <typeparamref name="ITEM"/></param>
		/// <returns>True if there is an item</returns>
		public bool TryDequeue(out ITEM item){
			lock(SyncLock){
				// If the head is empty
				if(Head == null){
					item = default(ITEM);
					return false;
				}

				// It is not empty
				item = Head.Item;
				Head = Head.Next;

				// If it is the last item
				if(Head == null) Tail = null;

				Items--;
			}

			return true;
		}

		/// <summary>Try to pick up an item without removing</summary>
		/// <param name="item">Item <typeparamref name="ITEM"/></param>
		/// <returns>True if there is an item</returns>
		public bool TryPeek(out ITEM item){
			lock(SyncLock){
				// If the head is empty
				if(Head == null){
					item = default(ITEM);
					return false;
				}

				// It is not empty
				item = Head.Item;
			}

			return true;
		}
	};
};