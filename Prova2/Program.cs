using System;

namespace Prova2
{
    class Program
    {
        //Variaveis globais
        static int opcao, nota, valorSaque = 0, quantidadeNota = 0, qDois = 0, qCinco = 0, qDez = 0, qVinte = 0,
            qCinquenta = 0, qCem = 0, qDuzentos = 0, opcaoNotasSaque = 0, saldoConta = 0, testeSaldo = 0, testeSaque, testeSaqueEspecifico,
            menosDuzentos = 0, menosCem = 0, menosCinquenta = 0, menosVinte = 0, menosDez = 0, menosCinco = 0, menosDois = 0;
        static bool senhaAprovada;

        //metodo principal
        static void Main(string[] args)
        {
            do
            {   //Mostrar menu de opções, validadar os dados e verificar se o usuário deseja continuar
                VerificarMenuPrincipal();
                do
                {   //Realizar os métodos conforme a opção informada
                    switch (opcao)
                    {
                        case 1: Depositar(); break;
                        case 2: Sacar(); break;
                        case 0: return;
                    }
                }
                while (true);
            }
            while (true);
        }

        /**********************************************************Metodo Principal de Saque**********************************************************/
        static Boolean Sacar()
        {
            InformarSaldoDisponivel();           
            if (saldoConta <= 0)
            {   //Se não houver saldo Mostrar mensagem, quantidade de notas e ir para menu principal
                InformarSaldoIndisponivel();
                InformarQuantidadeNotas();
                VerificarMenuPrincipal();
            }
            else
            {   
                if (!VerificarValorSaque())
                {   //se o a vericação do valor solicitado for negativa ele manda ela pro menu principal
                    VerificarMenuPrincipal();
                }
                else
                {   //senao ele pergunta ao usuário as notas que deseja receber
                    Console.WriteLine("Digite 1 para especificar as notas que deseja receber, 2 para prosseguir ou 0 para voltar");
                    if (!(int.TryParse(Console.ReadLine(), out opcaoNotasSaque)) || (opcaoNotasSaque != 1 && opcaoNotasSaque != 2 && opcaoNotasSaque != 0))
                    {
                        //Informar que opção é invalida e retornar o menu anterior
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção de saque inválida!\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        VerificarMenuPrincipal();
                    }
                    else
                    {   //selecionar a opção que o usuário deseja para o seu saque
                        switch (opcaoNotasSaque)
                        {   
                            case 1: SacarEspecifico(); break;
                            case 2: SacarSemEspecificar(); break;
                            case 0: VerificarMenuPrincipal(); break;
                        }

                    }
                }
            }
            return true;
        }

        /**********************************************************Metodo de Saque 1**********************************************************/
        static Boolean SacarEspecifico()
        {   
            if (valorSaque > saldoConta)
            {   //verificar se o valor do saque é maior que o saldo da conta
                InformarSaldoIndisponivel();
                VerificarMenuPrincipal();
            }
            else if (TestarSaqueEspecifico())
            {   
                senhaAprovada = SolicitarSenha();
                if (senhaAprovada)
                {   //senha confirmada então ele desconta as notas de fato e faz os demais métodos em sequencia
                    SacarEspecificoAprovado();
                    InformarSaqueSucesso();
                    InformarSaldoDisponivel();
                    InformarQuantidadeNotas();
                    VerificarMenuPrincipal();
                }
            }
            return true;
        }

        static bool TestarSaqueEspecifico()
        {   //variaveis do metodo
            testeSaqueEspecifico = valorSaque;
            int cedula = 200, quantidade = qDuzentos, quantidadeSaque;
            //verificar se hoje é quarta-feira
            cedula = cedula == 200 && (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday) ? 100 : 200;
            //variavel de controle
            bool saqueEspecifico = true, testeSaquePositivo = false;
            do
            {
                do
                {   //solicitar quantidade de notas de cada cédula
                    Console.WriteLine($"Digite a quantidade de notas de {cedula} reais:");
                    if (!(int.TryParse(Console.ReadLine(), out quantidadeSaque)) || quantidadeSaque < 0)
                    {   //se a conversão não der certa e a quantidade for menor que 0
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Quantidade inválida! Tente novamente.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (quantidadeSaque > quantidade)
                    {   //se a quantidade de saque daquela cédula for maior que a quantidade 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Quantidade de notas é superior a quantidade disponível!\n\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        testeSaquePositivo = true;
                        InformarQuantidadeNotas();
                        VerificarMenuPrincipal();
                        break;
                    }
                    else if ((cedula * quantidadeSaque) > testeSaqueEspecifico)
                    {   //se a cedula vezes a quantidade dela for maior que o valor a ser sacado
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Esta quantidade vai gerar um valor que supera o valor solicitado! Não é possível continuar!\n\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        VerificarMenuPrincipal();
                        break;
                    }
                    else
                    {   //descontar do teste a quantidade 
                        testeSaqueEspecifico -= (cedula * quantidadeSaque);
                        switch (cedula)
                        {   //verifica a nota e seta os valores para aquela nota e muda os campos de cedula e quantidade
                            case 200: menosDuzentos = quantidadeSaque; cedula = 100; quantidade = qCem; break;
                            case 100: menosCem = quantidadeSaque; cedula = 50; quantidade = qCinquenta; break;
                            case 50: menosCinquenta = quantidadeSaque; cedula = 20; quantidade = qVinte; break;
                            case 20: menosVinte = quantidadeSaque; cedula = 10; quantidade = qDez; break;
                            case 10: menosDez = quantidadeSaque; cedula = 5; quantidade = qCinco; break;
                            case 5: menosCinco = quantidadeSaque; cedula = 2; quantidade = qDois; break;
                            case 2: menosDois = quantidadeSaque; saqueEspecifico = false; break;
                        }
                        //resetar quantidade do saque para 0
                        quantidadeSaque = 0;
                    }
                }
                while (saqueEspecifico);
                //se o o saldo do teste for um valor diferente de zero e não tiver dado nenhum outro erro
                if (testeSaqueEspecifico != 0 && testeSaquePositivo == false)
                {   //imprimir erro
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Impossível compor o valor solicitado!\n\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    InformarQuantidadeNotas();
                    VerificarMenuPrincipal();
                    break;
                }
                break;
            }
            while (testeSaquePositivo);
            //ternario para verificar se teste deu certo e retornar
            bool saqueEspecificoAprovado = testeSaqueEspecifico != 0 ? false : true;
            return saqueEspecificoAprovado;
        }

        /**********************************************************Metodo de Saque 2**********************************************************/
        static void SacarEspecificoAprovado()
        {   //verificar a cedula e depois descontar a quantidade da mesma e ajustar o saldo da conta
            qDuzentos -= menosDuzentos;
            qCem -= menosCem;
            qCinquenta -= menosCinquenta;
            qVinte -= menosVinte;
            qDez -= menosDez;
            qCinco -= menosCinco;
            qDois -= menosDois;
            //ajustar o saldo da conta
            saldoConta = (qDuzentos * 200) + (qCem * 100) + (qCinquenta * 50) + (qVinte * 20) + (qDez * 10) + (qCinco * 5) + (qDois * 2);
        }

        static void SacarSemEspecificar()
        {
            //setar a variavel de teste
            testeSaque = valorSaque;
            //Verifica saldo 
            if (valorSaque > saldoConta)
            {   //se não houver ele informa o saldo indisponivel e vai para o menu principal
                InformarSaldoIndisponivel();
                VerificarMenuPrincipal();
            }
            //verificar se há notas disponiveis
            else if (!VerificarNotas())
            {
                //se não houver imprime a mensagem de erro, depois informa a quantidade de notas e manda para o menu principal
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Não foi possível montar o valor solicitado!\n");
                Console.ForegroundColor = ConsoleColor.White;
                InformarQuantidadeNotas();
                Console.WriteLine("");
                VerificarMenuPrincipal();
            }
            else
            {   //se a senha for true, ele vai prosseguir com o programa de saque
                senhaAprovada = SolicitarSenha();
                //setar a variavel de testeSaque para fazer o teste novamente com a senha aprovada e imprimir as notas
                testeSaque = valorSaque;
                if (senhaAprovada && VerificarNotas())
                {   //Realizar métodos de informações saque, saldo e quantidade de notas e ir para o menu principal
                    InformarSaqueSucesso();
                    InformarSaldoDisponivel();
                    InformarQuantidadeNotas();
                    Console.WriteLine("");
                    VerificarMenuPrincipal();
                }
            }
        }

        static Boolean VerificarValorSaque()
        {   //solicitar valor
            Console.WriteLine($"Digite o valor que deseja sacar:");
            if (!(int.TryParse(Console.ReadLine(), out valorSaque)) || valorSaque <= 0)
            {   //se o a conversão não for feita e o valor for menor que zero
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Valor inválido para saque!\n");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday && valorSaque > 800)
            {   //se for domingo e tentar sacar mais de 800 reais
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Operação não permitida! Aos domingos o valor máximo para saque é de R$ 800,00\n");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday && valorSaque > 1000)
            {   //se for sabado e tentar sacar mais de 1000
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Operação não permitida! Aos sábados o valor máximo para saque é de R$ 1.000,00\n");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else
            {   //tudo certo? sem erros? pode retornar como true
                return true;
            }

        }

        static Boolean VerificarNotas()
        {    //neste metodo eu faço uma simulação com as notas disponiveis
            testeSaldo = saldoConta;
            bool teste = true;
            int cedula = 200, quantidade = qDuzentos;
            //verificar se hoje é quarta-feira (sunday somente para teste porque ao mudar a data do meu note, não funcionou mais nada do debug)
            cedula = cedula == 200 && (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday) ? 100 : 200;
            while (teste)
            {   //aqui ele vai descontando e fazendo a simulação, enquanto a senha for falsa, quando for verdadeira, este método irá de fato descontar
                testeSaldo = SubtrairNotasSaque(testeSaldo, cedula, quantidade);
                switch (cedula)
                {   //passar a cédula e quantidade para a próxima nota a ser testada
                    case 200: cedula = 100; quantidade = qCem; break;
                    case 100: cedula = 50; quantidade = qCinquenta; break;
                    case 50: cedula = 20; quantidade = qVinte; break;
                    case 20: cedula = 10; quantidade = qDez; break;
                    case 10: cedula = 5; quantidade = qCinco; break;
                    case 5: cedula = 2; quantidade = qDois; break;
                    case 2: teste = false; break; //depois de testar a nota de 2 ele quebra o loop
                }
            }
            //Se o resultado no teste no while for diferente de 0 significa que não é possivel sacar 
            bool saqueStatus = testeSaque == 0 ? true : false;
            return saqueStatus;
        }

        static int SubtrairNotasSaque(int total, int cedula, int quantidade)
        {
            while (true)
            {   //cedula maior que o valor de saque, a quantidade maior que zero e não pode restar 1 na subtração do valor de saque menos a cédula
                if ((testeSaque >= cedula) && (quantidade > 0) && (testeSaque - cedula != 1))
                {   //desconta do total de saldo teste, do saque e da quantidade da cedula
                    total -= cedula;
                    testeSaque -= cedula;
                    quantidade--;                   
                    if (senhaAprovada)
                    {   //se a senha for digitada corretamente, será true e dai ele vai poder descontar da conta efetivamente
                        switch (cedula)
                        {   //verificar a cedula e depois descontar de 1 em um no mesmo loop a quantidade da mesma e o saldo da conta
                            case 200: qDuzentos--; saldoConta -= 200; break;
                            case 100: qCem--; saldoConta -= 100; break;
                            case 50: qCinquenta--; saldoConta -= 50; break;
                            case 20: qVinte--; saldoConta -= 20; break;
                            case 10: qDez--; saldoConta -= 10; break;
                            case 5: qCinco--; saldoConta -= 5; break;
                            case 2: qDois--; saldoConta -= 2; break;
                        }
                    }
                }
                else
                {   //se não se encaixar nas condições ele só da um break e retorna
                    break;
                }
            }
            return total;
        }

        /**********************************************************Metodo de Senha**********************************************************/
        static Boolean SolicitarSenha()
        {   //Variavel local
            int senha;
            //verificar se hoje é sabado e domingo e setar as tentativas para 3 ou 4            
            int tentativasSenha = (DateTime.Today.DayOfWeek == DayOfWeek.Sunday || DateTime.Today.DayOfWeek == DayOfWeek.Saturday) ? 3 : 4;
            do
            {   //Solicitar a senha para o usuario
                Console.WriteLine("Digite sua senha:");
                if (!(int.TryParse(Console.ReadLine(), out senha)) || (senha <= 10000 || senha >= 10060))
                {
                    //Verifica se a senha está certa
                    tentativasSenha--;
                    //Se as tentativas forem excedidas, imprimir mensagem de erro avisando que não é possivel sacar
                    if (tentativasSenha == 0)
                    {   //Informar que esgotou as tentativas de senha para aquele saque
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("As tentativas de digitação de senhas foram esgotadas. O Saque não poderá ser realizado!\n\n\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        //Ir para o menu principal e quebrar o loop
                        VerificarMenuPrincipal();
                        break;
                    }
                    //Se errar a senha, mas ainda tiver tentativas
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Senha inválida! Você ainda tem: {tentativasSenha} tentativa(s).\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {   //se tudo der certo ele quebra o loop setando as tentivas para zero do mesmo jeito
                    tentativasSenha = 0;
                }
            }
            while (tentativasSenha > 0);
            //verificar se deu bom a senha e retornar o bool
            bool verificacao = senha > 10000 && senha < 10060 ? true : false;
            return verificacao;
        }

        /*******************************************************Metodos de Informar o Usuário*******************************************************/
        static void InformarSaqueSucesso()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Saque realizado com sucesso!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        static void InformarSaldoDisponivel()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Saldo disponível: {saldoConta.ToString("C")}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void InformarQuantidadeNotas()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Quantidade disponível de notas de R$ 200: {qDuzentos}");
            Console.WriteLine($"Quantidade disponível de notas de R$ 100: {qCem}");
            Console.WriteLine($"Quantidade disponível de notas de R$ 50: {qCinquenta}");
            Console.WriteLine($"Quantidade disponível de notas de R$ 20: {qVinte}");
            Console.WriteLine($"Quantidade disponível de notas de R$ 10: {qDez}");
            Console.WriteLine($"Quantidade disponível de notas de R$ 5: {qCinco}");
            Console.WriteLine($"Quantidade disponível de notas de R$ 2: {qDois}");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void InformarSaldoIndisponivel()
        {   
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Saldo indisponível!\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /**********************************************************Metodos de Deposito**********************************************************/
        static void Depositar()
        {   //vaeriavel do método
            bool opcaoNota;
            //loop para obrigar o usuário digitar certo
            do
            {   //verificar o menu de notas para depósito
                opcaoNota = VerificarMenuNotas();
            }
            //se a opção digitada for invalida ou não existir ele continua o loop
            while (!opcaoNota);
            //verificar se a entrada informada foi zero
            if (nota == 0)
            {   //se for ele retorna pro menu principal
                VerificarMenuPrincipal();
            }
            else
            {   //se não ele vai depositar o valor informado
                DepositarValorInformado();
            }
        }
        static void DepositarValorInformado()
        {   //variavel do método
            bool opcaoQuantidade;
            //se a opção digitada for invalida ou não existir ele continua o loop
            do
            {   
                opcaoQuantidade = AtualizarNotasDepositos();
            }
            while (!opcaoQuantidade);
            //Verificar se a quantidade de notas foi igual a 0 -> vai para o método depositar 
            if (quantidadeNota == 0)
            {
                Depositar();
            }
            //ou deposita, atualiza e volta para o método depositar
            else
            {
                AtualizarQuantidadeNotasDeposito();
                Depositar();
            }
        }

        static void AtualizarQuantidadeNotasDeposito()
        {
            
            switch (nota)
            {   //atualizar a quantidade de notas de acordo com a nota informada e a quantidade
                case 2: qDois += quantidadeNota; saldoConta += (quantidadeNota * 2); break;
                case 5: qCinco += quantidadeNota; saldoConta += (quantidadeNota * 5); break;
                case 10: qDez += quantidadeNota; saldoConta += (quantidadeNota * 10); break;
                case 20: qVinte += quantidadeNota; saldoConta += (quantidadeNota * 20); break;
                case 50: qCinquenta += quantidadeNota; saldoConta += (quantidadeNota * 50); break;
                case 100: qCem += quantidadeNota; saldoConta += (quantidadeNota * 100); break;
                case 200: qDuzentos += quantidadeNota; saldoConta += (quantidadeNota * 200); break;
                case 0: VerificarMenuPrincipal(); break; //se acaso for 0 vai para o menu e quebra o loop atual
            }

            switch (nota)
            {   //atualizar a quantidade de notas a ser mostrada ao usuário
                case 2: quantidadeNota = qDois; break;
                case 5: quantidadeNota = qCinco; break;
                case 10: quantidadeNota = qDez; break;
                case 20: quantidadeNota = qVinte; break;
                case 50: quantidadeNota = qCinquenta; break;
                case 100: quantidadeNota = qCem; break;
                case 200: quantidadeNota = qDuzentos; break;
            }
            //Atualizar a quantidade de nota informada
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"A quantidade da nota de {nota} reais foi atualizada para:");
            Console.ForegroundColor = ConsoleColor.White;           
            Console.WriteLine($"{quantidadeNota}\n");
            quantidadeNota = 0;
        }

        static Boolean AtualizarNotasDepositos()
        {   //Solicitar a quantidade
            Console.WriteLine($"Digite uma quantidade para a nota de {nota} reais ou 0 para voltar ao menu anterior");
            //Validar que a nota é um inteiro
            if (!(int.TryParse(Console.ReadLine(), out quantidadeNota)) || quantidadeNota < 0)
            {   //informar mensagem de erro
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Quantidade inválida! Tente novamente.\n");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else
            {
                return true;
            }
        }
        /*******************************************************Verificar Menu Principal********************************************************/
        static void VerificarMenuPrincipal()
        {   //entrada de dados do menu
            bool opcaoInformada;
            do
            {   //Menu de Opções
                Console.WriteLine("Digite 1 para depositar, 2 para sacar um valor ou 0 para sair");
                //se a conversão e validações derem errado
                if (!(int.TryParse(Console.ReadLine(), out opcao)) || (opcao != 1 && opcao != 2 && opcao != 0))
                {   //informar mensagem de erro
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    opcaoInformada = false;
                }
                else
                {   //tudo certo ele quebra o loop
                    opcaoInformada = true;
                }
                //Se a opção for igual a zero ele já sai do programa
            } while (!opcaoInformada);
        }
        /*******************************************************Verificar Menu Notas Deposito********************************************************/
        static Boolean VerificarMenuNotas()
        {   //Menu de Notas
            Console.WriteLine("Digite uma nota para depositar ou 0 para voltar ao menu anterior.");
            //Converter e validar as opções de notas
            if (!(int.TryParse(Console.ReadLine(), out nota)) || (nota != 0 && nota != 2 && nota != 5 && nota != 10 && nota != 20 && nota != 50 && nota != 100 && nota != 200))
            {   //informar erro
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Valor inválido! Tente novamente.\n");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
