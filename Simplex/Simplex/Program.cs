using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static double[,] matrizSuperior = new double[200, 200];
        static double[,] matrizInferior = new double[200, 200];
        static int tamanhoColuna = 0, tamanhoLinha = 0, tamanhoColunaAux = 0;
        static double auxiliar = 0;
        static string tipo, leitura = "";
        static int colunaPermissiva = 0;
        static int linhaPermissiva = 0;
        static int[] variaveis = new int[100];
        static int indiceVariaveis = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Sua função é de maximização? Entre com \"max\".\nSe for de minimização entre com \"min\"");
            tipo = Console.ReadLine();
            if (tipo == "max" || tipo == "min")
            {
                comecaPrograma(tipo);
            }
            else
            {
                Console.WriteLine("Entrada incorreta");

            }
        }
        static void comecaPrograma(string tipo)
        {
            matrizSuperior[0, 0] = 0;
            tamanhoColuna++;
            variaveis[indiceVariaveis] = indiceVariaveis;
            indiceVariaveis++;
            while (leitura != "ql")
            {
                Console.WriteLine("Insira indices que acompanham as variáveis na ordem na função objetiva. \"ql\" para ir para as restrições");
                leitura = Console.ReadLine();
                if (leitura != "ql")
                {
                    if (tipo == "max")
                    {
                        matrizSuperior[tamanhoLinha, tamanhoColuna] = Convert.ToDouble(leitura);
                    }
                    else if (tipo == "min")
                    {
                        auxiliar = Convert.ToDouble(leitura);
                        matrizSuperior[tamanhoLinha, tamanhoColuna] = (auxiliar * -1);
                    }
                    tamanhoColuna++;
                    variaveis[indiceVariaveis] = indiceVariaveis;
                    indiceVariaveis++;
                }
            }
            while (leitura != "exit" && tipo != "exit")
            {
                while (leitura != "ql" && leitura != "exit")
                {
                    Console.WriteLine("Insira indices que acompanham as variáveis na ordem em que aparecem e digite \"ql\" para terminar a linha e passar para a próxima restrição. Digite \"exit\" quando acabarem as restrições");
                    leitura = Console.ReadLine();
                    if (leitura != "exit" && leitura != "ql")
                    {
                        if (tipo == "max")
                        {
                            auxiliar = Convert.ToDouble(leitura);
                            matrizSuperior[tamanhoLinha, tamanhoColuna] = (auxiliar * -1);

                        }
                        else if (tipo == "min")
                        {
                            matrizSuperior[tamanhoLinha, tamanhoColuna] = Convert.ToDouble(leitura);
                        }
                        tamanhoColuna++;
                    }
                }
                if (leitura == "ql")
                {
                    tamanhoLinha++;
                    tamanhoColunaAux = tamanhoColuna;
                    tamanhoColuna = 0;
                    Console.WriteLine("Proxima restrição será maior igual entre: \"max\".\nProxima restrição será menor igual entre: \"min\".Para sair digite \"exit\" ");
                    tipo = Console.ReadLine();
                    leitura = tipo;
                    variaveis[indiceVariaveis] = indiceVariaveis;
                    indiceVariaveis++;
                }


            }
            tamanhoLinha++;
            while (negativoML())
            {
                printaMatriz();
                parte1();
                parte3(colunaPermissiva);

                algoritmoDaTroca(linhaPermissiva, colunaPermissiva);
                printaMatriz();
                printaMatrizInf();
            }
            while (positivoPrimeiraLinha())
            {
                segundaFase();
            }

            Console.WriteLine("\n matriz final:");
            printaMatriz();
            Console.WriteLine("\n Respostas:\nO valor máximo possível de ser encontrado é " + Math.Abs(matrizSuperior[0, 0]) + " com as seguintes configuraçãoes de variáveis:\n");
            for (int a = 1; a < indiceVariaveis; a++)
            {
                if (a < tamanhoColunaAux)
                    Console.WriteLine("Variável de numero: " + variaveis[a] + " deve ter o valor: " + matrizSuperior[0, a] + "\n");
                else
                {
                    Console.WriteLine("Variável de numero: " + variaveis[a] + " deve ter o valor: " + matrizSuperior[((a - tamanhoColunaAux) + 1), 0] + "\n");
                }

            }

            loopa();
        }
        public static bool positivoPrimeiraLinha()
        {
            bool resp = false;
            for (int y = 1; y < tamanhoColunaAux; y++)
            {
                if (matrizSuperior[0, y] >= 0)
                {
                    resp = true;
                }
            }
            return resp;
        }

        public static void segundaFase()
        {
            bool continua = true;
            bool ilimitada = false;

            if (!multiplasSolucoes())
            {
                for (int y = 1; y < tamanhoColunaAux && continua; y++)
                {
                    if (matrizSuperior[0, y] > 0)
                    {
                        if (linhasPositivas(y))
                        {
                            colunaPermissiva = y;
                            menorQuocienteLinhas(y);
                            algoritmoDaTroca(linhaPermissiva, colunaPermissiva);
                            continua = false;
                        }
                        else
                        {
                            ilimitada = true;
                        }
                    }
                }
                if (continua == true && ilimitada == true)
                {
                    Console.WriteLine("Resposta Ilimitada");
                    loopa();
                }
            }
            else
            {
                Console.WriteLine("Multiplas soluções");
                loopa();
            }


        }
        //&& (matrizSuperior[x, y] != 0 && matrizSuperior[x, 0] != 0)
        public static void menorQuocienteLinhas(int y)
        {
            double respostaAux = Double.MaxValue;
            double valorAux = 0;
            for (int x = 1; x < tamanhoLinha; x++)
            {
                if (mesmoSinal(matrizSuperior[x, 0], matrizSuperior[x, y]))
                {
                    valorAux = (matrizSuperior[x, y] / matrizSuperior[x, 0]);
                    if (matrizSuperior[x, y] < respostaAux)
                    {
                        respostaAux = valorAux;
                        linhaPermissiva = x;
                    }
                }
            }
        }

        //conferir o valor > aqui na parte 3
        public static void parte3(int y)
        {
            double valor = Double.MaxValue;
            double valorAux = 0;
            for (int x = 1; x < tamanhoLinha; x++)
            {
                if (mesmoSinal(matrizSuperior[x, 0], matrizSuperior[x, y]))
                {
                    valorAux = (matrizSuperior[x, y] / matrizSuperior[x, 0]);
                    if (valorAux < valor)
                    {
                        valor = valorAux;
                        linhaPermissiva = x;
                    }
                }
            }
            permissiva(true);

        }

        public static bool linhasPositivas(int y)
        {
            bool resp = false;
            for (int x = 1; x < tamanhoLinha; x++)
            {
                if (matrizSuperior[x, y] > 0)
                {
                    resp = true;
                }
            }
            return resp;
        }


        public static bool multiplasSolucoes()
        {
            bool resposta = false, flag1 = false;
            bool flag2 = true;
            for (int y = 1; y < tamanhoColunaAux; y++)
            {
                if (matrizSuperior[0, y] == 0)
                {
                    flag1 = true;
                }

                else if (matrizSuperior[0, y] > 0)
                {
                    flag2 = false;
                }

            }
            if (flag1 && flag2)
            {
                resposta = true;
            }
            return resposta;
        }


        public static bool negativoML()
        {
            bool resp = false;
            for (int x = 1; x < tamanhoLinha; x++)
            {
                if (matrizSuperior[x, 0] < 0)
                {
                    resp = true;
                }
            }
            return resp;
        }

        public static void printaMatriz()
        {
            Console.Write("\nprinta superior:\n");
            for (int x = 0; x < tamanhoLinha; x++)
            {
                for (int y = 0; y < tamanhoColunaAux; y++)
                {
                    Console.Write(matrizSuperior[x, y] + " ");

                }
                Console.Write("\n");
            }

        }


        public static void printaMatrizInf()
        {
            Console.Write("\nprinta inferior:\n");
            for (int x = 0; x < tamanhoLinha; x++)
            {
                for (int y = 0; y < tamanhoColunaAux; y++)
                {
                    Console.Write(matrizInferior[x, y] + " ");

                }
                Console.Write("\n");
            }

        }

        public static void parte1()
        {
            int y = 0;
            bool entrou = false;
            for (int x = 1; x < tamanhoLinha && entrou == false; x++)
            {
                if (matrizSuperior[x, y] < 0)
                {
                    entrou = parte2(x);

                }
                if (entrou == false && x == tamanhoLinha - 1)
                {
                    permissiva(false);

                }
            }
            if (entrou == true)
            {
                permissiva(true);
            }

        }
        public static void permissiva(bool decisao)
        {
            if (!decisao)
            {
                Console.Write("\n Nao há resposta.");
                while (true)
                {

                }
            }
            else
            {
                Console.Write("\n coluna permissiva:" + colunaPermissiva);
                Console.Write("\n linha permissiva:" + linhaPermissiva);
            }
        }


        public static bool parte2(int x)
        {
            bool teste = false;
            for (int y = 1; y < tamanhoLinha && teste == false; y++)
            {
                if (matrizSuperior[x, y] < 0)
                {
                    colunaPermissiva = y;
                    teste = true;
                }
            }

            return teste;
        }




        public static bool mesmoSinal(double a, double b)
        {
            bool resposta = false;
            if ((a <= 0 && b <= 0) || (a >= 0 && b >= 0))
            {
                resposta = true;
            }
            return resposta;
        }


        public static void algoritmoDaTroca(int x, int y)
        {
            double centroCruzInf = 1 / matrizSuperior[x, y];

            //parte inferior da linha permissiva
            for (int yAux = 0; yAux < tamanhoColunaAux; yAux++)
            {
                matrizInferior[x, yAux] = matrizSuperior[x, yAux] * centroCruzInf;
            }

            //parte superior da coluna permissiva
            for (int xAux = 0; xAux < tamanhoLinha; xAux++)
            {
                matrizInferior[xAux, y] = matrizSuperior[xAux, y] * ((-1) * centroCruzInf);
            }

            //centro inferior
            matrizInferior[x, y] = centroCruzInf;

            //quadrante superior esquedo
            for (int xAux = 0; xAux < x; xAux++)
                for (int yAux = 0; yAux < y; yAux++)
                    matrizInferior[xAux, yAux] = matrizSuperior[x, yAux] * matrizInferior[xAux, y];
            //quadrante superior direito
            for (int xAux = 0; xAux < x; xAux++)
                for (int yAux = y + 1; yAux < tamanhoColunaAux; yAux++)
                    matrizInferior[xAux, yAux] = matrizSuperior[x, yAux] * matrizInferior[xAux, y];
            //quadrante inferior esquedo
            for (int xAux = x + 1; xAux < tamanhoLinha; xAux++)
                for (int yAux = 0; yAux < y; yAux++)
                    matrizInferior[xAux, yAux] = matrizSuperior[x, yAux] * matrizInferior[xAux, y];
            //quadrante inferior direito
            for (int xAux = x + 1; xAux < tamanhoLinha; xAux++)
                for (int yAux = y + 1; yAux < tamanhoColunaAux; yAux++)
                    matrizInferior[xAux, yAux] = matrizSuperior[x, yAux] * matrizInferior[xAux, y];

            //passagem para a nova matriz
            for (int xAux = 0; xAux < tamanhoLinha; xAux++)
            {
                for (int yAux = 0; yAux < tamanhoColunaAux; yAux++)
                {
                    matrizSuperior[xAux, yAux] = matrizInferior[xAux, yAux] + matrizSuperior[xAux, yAux];

                }
            }

            for (int xAux = 0; xAux < tamanhoLinha; xAux++)
            {
                matrizSuperior[xAux, y] = matrizInferior[xAux, y];
            }

            for (int yAux = 0; yAux < tamanhoColunaAux; yAux++)
            {
                matrizSuperior[x, yAux] = matrizInferior[x, yAux];
            }

            trocaVariavel(x, y);

        }

        public static void trocaVariavel(int x, int y)
        {
            int aux = 0;
            aux = variaveis[y];
            variaveis[y] = variaveis[tamanhoColunaAux + x];
            variaveis[tamanhoColunaAux + x] = aux;

        }

        public static void loopa()
        {
            while (true)
            {

            }

        }


    }
}