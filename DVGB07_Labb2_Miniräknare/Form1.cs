using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVGB07_Labb2_Miniräknare
{
    //Kevin Berling DVGB07 Labb 2 Miniräknare 2024-05-16
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<int> tal = new List<int>(); //ListArray för inmattade tal
        List<String> tecken = new List<String>(); //ListArray för inmatade tecken
        String ekvation = "";

        private bool isNewNumber = false; //För att dubbelkolla om nytt tals används eller ej
        private bool justCalculated = false; // För att kolla om likhetstecknet nyss användes

        private void btn_Equals_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ekvation))
                {
                    tal.Add(int.Parse(ekvation));
                    ekvation = "";
                }
                tecken.Add("=");

                int result = 0;
                bool checkResult = true;

                for (int i = 0; i < tal.Count; i++)
                {
                    if (i == 0)
                    {
                        result = tal[i];
                    }
                    else
                    {
                        result = PerformOperation(result, tal[i], tecken[i - 1], ref checkResult);
                        if (!checkResult)
                        {
                            ClearAll();
                            ErrorMessage.Text = "Ogiltig operation";
                            return;
                        }
                    }
                }

                if (checkResult)
                {
                    txb_Display.Text = $"{result}";
                    ekvation = $"{result}";
                    isNewNumber = true; // Resultatet visas nu
                    justCalculated = true; // Markerar att likhetstecknet nyss användes
                }
                tal.Clear();
                tecken.Clear();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
                ClearAll();
                return;
            }
        }

        private void ClearAll()
        {
            txb_Display.Text = "";
            ErrorMessage.Text = "";
            ekvation = "";
            tal.Clear();
            tecken.Clear();
            justCalculated = false;
        }

        private int PerformOperation(int tal1, int tal2, string operation, ref bool checkResult) // Kollar om operationen lyckades eller ej
        {
            int result = 0;
            switch (operation)
            {
                case "+":
                    result = addera(tal1, tal2, ref checkResult);
                    break;
                case "-":
                    result = sub(tal1, tal2, ref checkResult);
                    break;
                case "X":
                    result = mul(tal1, tal2, ref checkResult);
                    break;
                case "/":
                    result = div(tal1, tal2, ref checkResult);
                    break;
                default:
                    ErrorMessage.Text = "Ogiltig operation";
                    checkResult = false;
                    break;
            }
            return result;
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void AddDigit(string digit)
        {
            if (isNewNumber)
            {
                txb_Display.Text = "";
                ekvation = "";
                isNewNumber = false;
            }
            ErrorMessage.Text = "";
            txb_Display.Text += digit;
            ekvation += digit;
            justCalculated = false; // När en ny siffra läggs till efter ett resultat, går vi tillbaka till normalt läge
        }

        private void AddOperator(string operatorSymbol)
        {
            try // Fångar upp om användaren matar in ett tal större än IntMax
            {
                if (!string.IsNullOrEmpty(ekvation))
                {
                    tal.Add(int.Parse(ekvation));
                    ekvation = "";
                }
                else if (justCalculated)
                {
                    tal.Add(int.Parse(txb_Display.Text));
                }
                txb_Display.Text += operatorSymbol;
                tecken.Add(operatorSymbol);
                justCalculated = false; // En operator läggs till, går tillbaka till normalt läge
            }
            catch (Exception)
            {
                ErrorMessage.Text = "Inmatade värdet är för stort";

                txb_Display.Text = "";
                ekvation = "";
                tal.Clear();
                tecken.Clear();
            }
        }

        //Hittade smidigare sätt i hobby projekt för att lösa siffrorna så jag implementerade det till min gamla kod
        private void btn_One_Click(object sender, EventArgs e) => AddDigit(btn_One.Text);
        private void btn_Two_Click(object sender, EventArgs e) => AddDigit(btn_Two.Text);
        private void btn_Three_Click(object sender, EventArgs e) => AddDigit(btn_Three.Text);
        private void btn_Four_Click(object sender, EventArgs e) => AddDigit(btn_Four.Text);
        private void btn_Five_Click(object sender, EventArgs e) => AddDigit(btn_Five.Text);
        private void btn_Six_Click(object sender, EventArgs e) => AddDigit(btn_Six.Text);
        private void btn_Seven_Click(object sender, EventArgs e) => AddDigit(btn_Seven.Text);
        private void btn_Eight_Click(object sender, EventArgs e) => AddDigit(btn_Eight.Text);
        private void btn_Nine_Click(object sender, EventArgs e) => AddDigit(btn_Nine.Text);
        private void btn_Zero_Click(object sender, EventArgs e) => AddDigit(btn_Zero.Text);

        private void btn_Addition_Click(object sender, EventArgs e) => AddOperator(btn_Addition.Text);
        private void btn_Subtraction_Click(object sender, EventArgs e) => AddOperator(btn_Subtraction.Text);
        private void btn_Multiplication_Click(object sender, EventArgs e) => AddOperator(btn_Multiplication.Text);
        private void btn_Division_Click(object sender, EventArgs e) => AddOperator(btn_Division.Text);

        private int addera(int tal1, int tal2, ref bool checkResult)
        {
            try
            {
                return checked(tal1 + tal2);
            }
            catch (OverflowException)
            {
                ErrorMessage.Text = "Summan överskrider maxvärdet!";
                checkResult = false;
                return 0;
            }
        }

        private int sub(int tal1, int tal2, ref bool checkResult)
        {
            try
            {
                return checked(tal1 - tal2);
            }
            catch (OverflowException)
            {
                ErrorMessage.Text = "Differensen överskrider minvärdet!";
                checkResult = false;
                return 0;
            }
        }

        private int mul(int tal1, int tal2, ref bool checkResult)
        {
            try
            {
                return checked(tal1 * tal2);
            }
            catch (OverflowException)
            {
                ErrorMessage.Text = "Produkten överskrider maxvärdet!";
                checkResult = false;
                return 0;
            }
        }

        private int div(int tal1, int tal2, ref bool checkResult)
        {
            if (tal2 == 0)
            {
                ErrorMessage.Text = "Division med noll är inte tillåten!";
                checkResult = false;
                return 0;
            }

            return tal1 / tal2;
        }
    }
}
