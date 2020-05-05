using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace RadomizedTree
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      for (int j = 0; j < 50; j++)
      {
        MyTree A = new MyTree();
        MyTree B = new MyTree();

        Random rnd = new Random();
        var array = Enumerable.Range(1, 20).OrderBy(i => rnd.Next()).ToArray();
        var array2 = Enumerable.Range(5, 10).OrderBy(i => rnd.Next()).ToArray();
        //436127
        foreach (var VARIABLE in array)
        {
          A.AddRandom(VARIABLE);
        }

        foreach (var VARIABLE in array2)
        {
          B.AddRandom(VARIABLE);
        }

        Console.WriteLine("Tree A\n");
        TreePrinter.PrintMyTree(A);
        Console.WriteLine();
        var t = A.Straite(A.Root);

        Console.WriteLine("\nTree B\n");
         TreePrinter.PrintMyTree(B);
        B.Symmetric(B.Root);

        A.Delete(A.Root, B);
       Console.WriteLine("\nTree A\n");
       TreePrinter.PrintMyTree(A);
      }

    }

    public class MyTree
    {
      public int Size { get; set; }
      public MyNode Root { get; set; }


      public MyNode Straite(MyNode node)
      {
        if (node == null) return null;
        if (node == Root)
        {
          Console.Write($"{node.Key} ");
          Straite(node.L_Son);
        }
        else
        {
          if (node.L_Son != null)
          {
            Console.Write($"{node.Key} ");
            Straite(node.L_Son);
          }
          else
          {
            if (node.R_Bro != null)
            {
              Console.Write($"{node.Key} ");
              while (node.R_Bro == null)
              {
                node = GetParet(node);
              }

              Straite(node.R_Bro);
            }

            if (node.L_Son == null && node.R_Bro == null)
            {
              Console.Write($"{node.Key} ");
              while (node != null && node.R_Bro == null)
                node = GetParet(node);
              if (node == Root) return null;
              if (node != null) Straite(node.R_Bro);
            }
          }
        }

        return null;
      }

      public void Symmetric(MyNode node1)
      {

        if (node1 == null) return;
       if(node1.L_Son!=null && node1.Key>node1.L_Son.Key)
         Symmetric(node1.L_Son);
       Console.Write($"{node1.Key} ");   
        if(node1.L_Son?.R_Bro!=null)  
          Symmetric(node1.L_Son?.R_Bro);
          
        else   
          Symmetric(node1.L_Son);

      }

      public void Delete(MyNode A, MyTree B)
      {
        if (A == null) return;
        bool found = B.Search(A.Key);

        if (!found)
        {

          if (A.L_Son == null)
          {
            MyNode parent = GetParet(A);
            if (parent != null)
            {
              if (parent.L_Son.Key == A.Key)
                parent.L_Son = A?.R_Bro;
              else
              {
                parent.L_Son.R_Bro = null;
              }
            }

            Delete(parent.L_Son, B);
            return;

          }

          if (A.L_Son != null && A.L_Son.R_Bro == null)
          {
            MyNode parent = GetParet(A);
            if (parent != null)
            {
              if (A.L_Son != null)
              {
                if (parent.L_Son.Key == A.Key)
                {
                  parent.L_Son = A?.L_Son;
                  parent.L_Son.R_Bro = A?.R_Bro;
                  Delete(parent.L_Son, B);
                  return;
                }
                else
                {
                  parent.L_Son.R_Bro = A?.L_Son;
                  Delete(parent.L_Son.R_Bro, B);
                }
              }
              else
                parent.L_Son = A?.R_Bro;

              return;
            }
            else
            {
              Root.Key = A.L_Son.Key;
              Root.L_Son = A?.L_Son.L_Son;
              Delete(Root, B);
              return;
            }
          }

          if (A.L_Son != null && A.L_Son.R_Bro != null)
          {
            MyNode parent = GetParet(A);
            MyNode temp = A.L_Son.R_Bro;

            while (temp.L_Son != null && temp.L_Son.Key < temp.Key)
            {
              temp = temp.L_Son;
            }

            MyNode parent2 = GetParet(temp);
            parent2 = GetParet(temp);

            if (parent2.Key != A.Key)
            {
              if (temp.L_Son != null)
              {
                parent2.L_Son = temp.L_Son;
                parent2.L_Son.R_Bro = temp.R_Bro;
              }
              else parent2.L_Son = temp?.R_Bro;

            }
            else
            {

              parent2.L_Son.R_Bro = temp?.L_Son;
            }

            if (parent != null)
            {
              A.Key = temp.Key;
              Delete(A, B);
              return;
            }
            else
            {
              Root.Key = temp.Key;
              Root.L_Son.R_Bro = A.L_Son?.R_Bro;
              Delete(Root, B);
              return;
            }

          }
        }

        if (A.L_Son != null)
        {
          Delete(A.L_Son, B);
        }

        if (A.R_Bro != null)
        {
          Delete(A.R_Bro, B);
        }

        if (A.R_Bro == null)
        {
          while (A != null && A.R_Bro == null)
            A = GetParet(A);
          if (A == Root) return;
          if (A == null) return;
          if (A != null)
          {
            Delete(A.R_Bro, B);
            return;
          }

        }

        return;

      }

      public bool Search(int toFind)
      {
        bool inTree = false;
        if (toFind == Root.Key) return true;
        recurtion(Root);

        void recurtion(MyNode node)
        {
          if (node == null) return;

          if (node.Key == toFind)
          {
            inTree = true;
            return;
          }

          if (toFind < node.Key)
          {

            if (node.L_Son == null)
              return;

            if (node.L_Son.Key < toFind)
            {
              if (node.L_Son.L_Son.R_Bro == null) recurtion(node.L_Son);
              recurtion(node.L_Son.L_Son.R_Bro);
              return;
            }

            recurtion(node.L_Son);

          }
          else
          {

            if (node.L_Son == null) return;


            if (node.L_Son.Key > toFind)
            {
              recurtion(node.L_Son);
              return;
            }

            if (node.L_Son.R_Bro == null && node.L_Son != null)
            {
              recurtion(node.L_Son);
            }

            recurtion(node.L_Son.R_Bro);
          }
        }

        return inTree;
      }

      public void InsertRoot(MyNode myNode)
      {
      
        while (myNode.Key != Root.Key)
        {
          MyNode parent = GetParet(myNode);

          if (parent.Key > myNode.Key)
            RotateR(myNode, parent);
          else
            RotateL(myNode, parent);

          if (parent.Key == Root.Key)
          {
            Root = myNode;
            break;
          }

        }

      }

      void RotateR(MyNode myNode, MyNode parent)
      {
        MyNode A = myNode.L_Son;
        MyNode B = A?.R_Bro;
        MyNode C = myNode.R_Bro;

        if (A != null && A.Key > myNode.Key)
        {
          B = A;
          A = null;
        }

        if (A != null) A.R_Bro = parent;
        if (B != null)
        {
          parent.L_Son = B;
          B.R_Bro = C;
        }
        else
        {
          parent.L_Son = C;
        }

        if (A == null)
        {
          myNode.L_Son = parent;
        }
        else
        {
          A.R_Bro = parent;
        }

        myNode.R_Bro = parent.R_Bro;
        MyNode grandpa = GetParet(parent);
        if (grandpa == null) return;
        if (grandpa.L_Son.Key == parent.Key) grandpa.L_Son = myNode;
        else grandpa.L_Son.R_Bro = myNode;

      }

      void RotateL(MyNode myNode, MyNode parent)
      {
        MyNode A = parent.L_Son;
        MyNode B = myNode.L_Son;
        MyNode C = B?.R_Bro;

        if (parent.L_Son.Key == myNode.Key)
        {
          A = null;
        }

        if (B != null && B.Key > myNode.Key)
        {
          C = B;
          B = null;
        }

        if (A != null)
        {
          A.R_Bro = B;
        }
        if (B != null)
        {
          B.R_Bro = null;
        }
        parent.L_Son = A ?? B;
        myNode.R_Bro = parent.R_Bro;
        parent.R_Bro = C;
        myNode.L_Son = parent;

        MyNode grandpa = GetParet(parent);
        if (grandpa == null) return;
        if (grandpa.L_Son.Key == parent.Key) grandpa.L_Son = myNode;
        else grandpa.L_Son.R_Bro = myNode;

      }

      public MyNode GetParet(MyNode me)
      {
        MyNode checking = Root;

        if (me == Root) return null;

        CheckParent();
        return checking;

        void CheckParent()
        {
          if (checking == null) return;

          MyNode bro = null;
          MyNode child = checking.L_Son;
          if (child != null)
          {
            bro = child.R_Bro;
          }

          if ((child != null && child.Key == me.Key) || (bro != null && bro.Key == me.Key)) return;
          if (checking.Key < me.Key && bro != null)
            checking = bro;
          else
            checking = child;
          CheckParent();
        }

      }

      public void AddRandom(int key)
      {

        MyNode adding = new MyNode(key);
        Add(adding);
        var random = new Random().Next(1, Size + 1);
        if (random == Size)
          InsertRoot(adding);
      }

      public void Add(MyNode k)
      {
        Size++;

        if (Root == null)
        {
          Root = new MyNode(k.Key);
          return;
        }

        recurtion(Root);

        void recurtion(MyNode node)
        {
          if (k.Key < node.Key)
          {
            if (node.L_Son == null)
            {
              node.L_Son = k;


              return;
            }

            if (node.L_Son.Key < node.Key)
            {
              recurtion(node.L_Son);
              return;
            }

            MyNode bro = node.L_Son;
            node.L_Son = k;
            node.L_Son.R_Bro = bro;

          }
          else
          {
            if (node.L_Son == null)
            {
              node.L_Son = k;

              return;
            }

            if (node.L_Son.Key > node.Key)
            {
              recurtion(node.L_Son);
              return;
            }

            if (node.L_Son.R_Bro == null)
            {
              node.L_Son.R_Bro = k;

              return;
            }

            recurtion(node.L_Son.R_Bro);
          }
        }


      }
    }

    public class MyNode
    {
      public int Key { get; set; }
      public MyNode L_Son { get; set; }
      public MyNode R_Bro { get; set; }


      public MyNode(int key)
      {
        Key = key;
      }
    }
  }
}