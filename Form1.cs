using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TeorFormalLanguagelaba2
{
    public partial class Form1 : Form
    {
        String StrIn;
        String StrOut;
        List<String> AssembCode;
        List<String> GoodAssembCode;
        public Form1()
        {
            InitializeComponent();
            
            
        }
        
        public static String ReversePolish(String input) 
        {
    if (input == null)
    	return "";
    Char[] inchar = input.ToCharArray();
    Stack<Char> stack = new Stack<Char>();
    String outchar = "";
            for (int i = 0; i < inchar.Length; i++) 
                switch (inchar[i])
                {
                    case '+':
                    case '-':
                        while (stack.Count != 0 && (stack.Peek() == '*' || stack.Peek() == '/'))
                        {

                            outchar+= " " + stack.Pop().ToString();
                        }
                        outchar += " ";
                        stack.Push(inchar[i]);
                        break;
                    case '*':
                    case '/':
                        outchar += " ";
                        stack.Push(inchar[i]);
                        break;
                    case '(':
                        stack.Push(inchar[i]);
                        break;
                    case ')':
                        while (stack.Count != 0 && stack.Peek() != '(')
                        {
                            outchar+= " " + stack.Pop().ToString();
                            
                        }
                        stack.Pop();
                        break;
                    default:
                        outchar+=inchar[i];
                        break;
                }
        while (stack.Count != 0)
            outchar += " " + stack.Pop().ToString();
    return outchar.ToString();
        }
        public static List<String> AssemblerCode(String input)
        {
            List<String> codebank=new List<String>();
            Char[] inchar = input.ToCharArray();
            String cnst="";
           
                 for (int i = 0; i < inchar.Length; i++)
                     switch (inchar[i])
                     {
                         case ' ':
                             if (cnst != "")
                             {
                                 codebank.Add("mov eax," + cnst);
                                 codebank.Add("push eax");
                                 cnst = "";
                             }
                             break;
                         case '+':                       
                            
                         case '-':
                             
                         case '*':
                             
                         case '/': 
                             codebank.RemoveAt(codebank.Count-1);
                             codebank.Add("pop edx");
                             codebank.Add("xchg eax,edx");
                         switch(inchar[i])
                         {
                             case '+':
                             codebank.Add("add eax,edx");
                             break;
                             case '-':
                             codebank.Add("sub eax,edx");
                             break;
                             case '*':
                             codebank.Add("imul eax,edx");
                             break;
                             case '/':
                             codebank.Add("idiv eax,edx");
                             break;
                        }
                             codebank.Add("push eax");
                             break;
                         default:
                             cnst += inchar[i];
                             break;


                     }
                 codebank.RemoveAt(codebank.Count - 1);
                 codebank.Add("mov A, eax");

                 return codebank;

        }
        public static List<String> GoodAssemblerCode(List<String> codebank)
        {
            List<String> goodcodebank = new List<String>();
            String boof1;
            String boof2;
            Boolean nomatch;
            Boolean nomatch2;
            int countpop;
            int countpush;
          
            nomatch = true;
            nomatch2 = true;
          for(int m=0;m<4;m++)
            {
                nomatch = true;
                nomatch2 = true;
                countpop = 0;
                countpush = 0;//C-(A+13)*D
                boof1 = "";
                boof2 = "";
               
                for (int i = 0; i < codebank.Count - 6; i++)
                {
                    
                    if (codebank[i].Contains("mov eax,"))
                        if (codebank[i + 1].Contains("push eax"))
                        {
                            for (int j = i + 2; j < codebank.Count - 3 && nomatch; j++)
                            {
                                    if (codebank[j].Contains("push")) nomatch=false;
                                    if (codebank[j].Contains("pop edx"))
                                    if (codebank[j + 1].Contains("xchg eax,edx"))
                                        if (codebank[j + 2].Contains("eax,edx") &&  nomatch)
                                        {
                                            nomatch = false;
                                            boof1 = codebank[i];
                                            boof1 = boof1.Substring(9, boof1.Length - 9);
                                            boof2 = codebank[j + 2];
                                            boof2 = boof2.Substring(0, 4);
                                            if (boof2.Length == 4)
                                                if (boof2[3] == ' ')
                                                    boof2 = boof2.Substring(0, 3);
                                            goodcodebank.Clear();
                                            for (int k = 0; k < i; k++)
                                                goodcodebank.Add(codebank[k]);
                                            for (int k = i + 2; k < j; k++)
                                                goodcodebank.Add(codebank[k]);
                                                goodcodebank.Add("mov edx," + boof1);
                                                goodcodebank.Add("xchg eax,edx");
                                                goodcodebank.Add(boof2 + " eax,edx");
                                            for (int k = j + 4; k < codebank.Count; k++)
                                                goodcodebank.Add(codebank[k]);
                                            codebank.Clear();
                                            for (int l = 0; l < goodcodebank.Count; l++)
                                                codebank.Add(goodcodebank[l]);






                                        }
                                            
                            }
                        }
                   if (codebank[i].Contains("push eax") && !nomatch)
                        if (codebank[i + 1].Contains("mov eax,"))
                            if (codebank[i + 2].Contains("pop edx"))
                                if (codebank[i + 3].Contains("xchg eax,edx"))
                                    if (codebank[i + 4].Contains("eax,edx"))
                                    {
                                        nomatch2 = false;
                                        boof1 = codebank[i + 1];
                                        boof1 = boof1.Substring(8, boof1.Length - 8);
                                        boof2 = codebank[i + 4];
                                        boof2 = boof2.Substring(0, 4);
                                        if (boof2.Length == 4)
                                            if (boof2[3] == ' ')
                                                boof2 = boof2.Substring(0, 3);
                                        goodcodebank.Clear();
                                        for (int k = 0; k < i; k++)
                                            goodcodebank.Add(codebank[k]);
                                        goodcodebank.Add("mov edx," + boof1);
                                        goodcodebank.Add(boof2 + " eax,edx");

                                        for (int k = i + 5; k < codebank.Count; k++)
                                            goodcodebank.Add(codebank[k]);
                                        codebank.Clear();
                                        for (int l = 0; l < goodcodebank.Count; l++)
                                            codebank.Add(goodcodebank[l]);
                                    }

                    
                }
            /*   for (int i = 0; i < codebank.Count - 5 && nomatch2; i++)
                {
                    if (codebank[i].Contains("push eax"))
                        if (codebank[i+1].Contains("mov eax,"))
                            if (codebank[i+2].Contains("pop edx"))
                                if (codebank[i+3].Contains("xchg eax,edx"))
                                    if (codebank[i+4].Contains("eax,edx"))
                                    {
                                        nomatch2 = false;
                                        boof1 = codebank[i+1];
                                        boof1 = boof1.Substring(8, boof1.Length - 8);
                                        boof2 = codebank[i + 4];
                                        boof2 = boof2.Substring(0, 4);
                                        if (boof2.Length == 4)
                                            if (boof2[3] == ' ')
                                                boof2 = boof2.Substring(0, 3);
                                        goodcodebank.Clear();
                                        for (int k = 0; k < i; k++)
                                            goodcodebank.Add(codebank[k]);
                                        goodcodebank.Add("mov edx,"+boof1);
                                        goodcodebank.Add(boof2 + " eax,edx");

                                        for (int k = i + 5; k < codebank.Count; k++)
                                            goodcodebank.Add(codebank[k]);
                                        codebank.Clear();
                                        for (int l = 0; l < goodcodebank.Count; l++)
                                            codebank.Add(goodcodebank[l]);
                                    }
                }*/
              codebank.Clear();
              for(int l=0;l<goodcodebank.Count;l++)              
               codebank.Add(goodcodebank[l]);
                
              
           }
            
            return goodcodebank;
        }
        public bool optim1(List<String> code)
    {
        return false;
    }
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            StrIn = textBox1.Text;
            StrOut= ReversePolish(StrIn);
            textBox2.Text = StrOut;
            AssembCode = AssemblerCode(StrOut);
            for (int i = 0; i < AssembCode.Count; i++)
                listBox1.Items.Add(AssembCode[i]);
            GoodAssembCode = GoodAssemblerCode(AssembCode);
            for (int i = 0; i < GoodAssembCode.Count; i++)
                listBox2.Items.Add(GoodAssembCode[i]);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
