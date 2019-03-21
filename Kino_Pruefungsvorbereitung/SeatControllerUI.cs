﻿using CinemaSeatController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace Kino_Pruefungsvorbereitung
{
    public partial class SeatControllerUI : Form
    {
        private int[] angaben;              // Private bedeutet das man von dieser Klasse auf die Variable zugreifen kann, würde man jetzt noch einen "getter" dafür schreiben (public int[] getAngaben(){ return [this.]angaben }) könnte man diese auch in anderen klassen benutzten solange man die instance dieser klasse hat. Man könnte das ganze auch einfach ohne "private" machen, wir wollen aber hier doch die richtigen conventions verwenden.
        private String uhrzeit;             // ^
        


        private int price = 0;


        private IList<Sitzplatz> sitzplaetze;                   //Sitzplätze in "Liste" statt Array weil es für mich persönlich, einfacher war das Ziel zu erreichen.
  

        public SeatControllerUI(int[] angaben, String uhrzeit) //Inhalt des Fensters. (Sitze, Preis etc)

        {

            //Initialisierung, Deklaration und Instanzbildung
            sitzplaetze = new List<Sitzplatz>();                // init der liste

            int seats = angaben[0] * angaben[1];                // berechnung der gesamten sitze
            int seatsPerRow = angaben[1];                       // wie viele sitze in einer Reihe sind, bekommt Wert aus "Saal"etc Variable aus "program.cs" class
            int currentRow = 1;                                 // momentane reihe 
            price = 0;                                      
            int currentSeatsInRow = 0;                          // momentane sitze in momentaniger reihe 

            for (int i = 1; i <= seats; i++)                    //for schleide fürs erstellen der Sitze
            {

                bool premium = false;                           //Bool hat nur "true und false" als Wert. In diesem Fall hat "premium" standartmäßig den Wert "false"                     

                if (currentSeatsInRow == seatsPerRow)           //Wenn "CurrentSeatsInRow" dem Wert von "seatsPerRow" entspricht wird:
                {
                    currentRow++;                               //"eine Reihe weiter gegangen"
                    currentSeatsInRow = 0;                      //und der Wert für "currentSeatsInRow" wird auf 0 gesetzt, fängt also von neu an.
                }

                if (currentRow <= 5)                            //Falls der Wert von "currenRow" unter 5 ist, also die Reihe, die momentan erstellt wird geringer oder gleich 5 ist, wird:
                {
                    premium = true;                             //das Feld als "premium" markiert, sprich, es wird später als "günstiger" angepriesen und in einer anderen Farbe dargestellt.
                }
                Sitzplatz seat = new Sitzplatz(premium, currentRow,i);             
                sitzplaetze.Add(seat);
                //Console.WriteLine("New Seat created! Premium: " + premium + ", Row: " + seat.getReihe() + ", Number: " + i);      //Nachricht, welche ausgibt, das ein Sitz erstellt wurde.
                currentSeatsInRow++;
            }

            this.uhrzeit = uhrzeit;                             //verwenden der privaten Variable "uhrzeit"
            this.angaben = angaben;                             //verwenden der privaten Variable, sogar Arrays, "angaben"
            InitializeComponent();                              //Wird automatisch von "windows forms" erstellt.

            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Fixieren der Größe des grafischen Fensters.
            this.MaximizeBox = false;                           //^, "MaximizeBox" entscheidet ob das ein Vollbild-Fenster ist.
            resizeForm();                                       //wird später verwendet um die Größe des Fensters festzulegen.
            addSitzplaetze();                                   //Hier wird später die Position der Sitzplätze festgelegt
            setPriceLocation();                                 //Position des Gesamtpreises
            setPrintLocation();                                 //Position des Knopfes welcher alles in die Textddatei schreibt.
        }

        public void resizeForm()                                //Hier drin wird die Größe des Fensters bestimmt.
        {
            int[] size = new int[2];                            //Array für die Größe des Fensters, 
            size = calculateFormSize();                         //Das Array "size" bekommt die selben Werte zugewiesen wie das Array "calculateFormSize".
            this.Width = size[0];                               //Die Breite des Fensters bekommt den Wert des Arrays an Stelle "0", dies entspricht dem ersten Wert des Arrays.
            this.Height = size[1];                              //Die Höhe des Fensters bekommt den Wert des Arrays an Stelle "1", dies entspricht dem zweiten Wert des Arrays.
            this.Update();                                      //Update/Erneuern des ganzen.
        }

        public void setPrintLocation()                          //Festlegen der Position des Knopfes der fürs Ausgeben verantwortlich ist.
        {
            int x = this.Width;                                 //Die X-Koordinate wird gleichgesetzt mit  "Width"
            int y = this.Height;                                //Die Y-Koordinate wird gleichgesetzt mit  "Height"
            y -= 100;                                           //y wird -100 gerechnet
            x -= 125;                                           //x wird -125 gerechnet

            printButton.Location = new Point(x, y);                         //Die Position des Knopfes ist bei den Werten von y und x.

            printButton.Click += new EventHandler(printButtonClickEvent);   //Hier wird hinzugefügt, dass etwas passiert. Was genau passiert with in "protected void printButtonClickEvent(object sender, EventArgs e) gesagt.
            
        }

        protected void printButtonClickEvent(object sender, EventArgs e)    //Hier steht, was passiert wenn der "print" Knopf angeklickt wird.
        {
            string dateiPfad = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Kino Ticket.csv");
            StreamWriter sw = new StreamWriter(dateiPfad);            //Erstellen einer Funktion, welche es dem Programm ermöglich, sachen in eine Textdatei zu schreiben.


            

            //Man könnte den folgenden Part auch ohne Arrays machen, wird aber aus Lernzwecken mit Arrays gemacht.

            int b = 0;                                                  //b, dies entspricht der Anzahl der ausgewählten Sitzplätze.
            for(int i = 0; i < sitzplaetze.Count(); i++) {              //Jedes mal, wenn der print-knopf gedrückt wird, zählt die for-schleife durch.

                Sitzplatz sitzplatz = sitzplaetze.ElementAt(i);         //Dem Array "sitzplatz" wird der Wert eines Ausgewählten Sitzplatzes zugewiesen,

                    if (!sitzplatz.istVerfuegbar())                     //Falls der Sitzplatz verfügbar ist.
                    b++;                                                //b wird um "1" erhöht.
            }

            if(b == 0)
            {
                MessageBox.Show("Du hast keine Plätze ausgewählt!"); // "Errormessage" wenn keine Sitzplätze ausgewählt wurden. "MessageBox" ist ein kleines Fenster, welches sich öffnet und dann die Nachricht anzeigt.
                return;
            }

            Sitzplatz[] sitzplatzArray = new Sitzplatz[b];          //Array in welchem die ausgewwöhlten Sitzplätze gespeichert werden um dann ausgegeben zu werden.

            b = 0;                                                  
            for(int i = 0; i< sitzplaetze.Count(); i++)             //for-Schleife zum speichern der Sitzplätze in einem Array.
            {
                Sitzplatz sitzplatz = sitzplaetze.ElementAt(i);     //Array bekommt den Wert von dem Wert "i" im Array "sitzplaetze". "ElementAt" besorgt einen Wert and der angegebenen "
                if (!sitzplatz.istVerfuegbar())                     //Der Sitzplatz wird nur gespeichert, falls dieser "verfügbar" ist.         
                {

                    sitzplatzArray[b] = sitzplatz;                  //Speichern des Wertes
                    b++;                                            //Erhöhen des Wertes "b"
                }
            }


            // Hier wird ein StringBuilder benutzt um die Plätze in einem String aufzulisten.

            StringBuilder stringBuilder = new StringBuilder();      //"StringBuilder" um den String zur Ausgabe des Sitzplatzes zu "bauen".


            bool c = false;                                         //Boolean "c", beteiligt am Prozess des Erstellens des Strings

            foreach(Sitzplatz sitzplatz in sitzplatzArray)          //
            {
                if(c == false) {                                    //falls "c" false ist wird:
                stringBuilder.Append(sitzplatz.getNumber());        //Append für Zeichenfolge an String an. Hier wird nur die Nummer des Sitzplatzes in dem String gespeichert.
                c = true;                                           //und "c" auf "true" gesetzt 
                }
                else                                                                //Falls "c" nicht auf "false" ist
                {
                    stringBuilder.Append(",").Append(" " + sitzplatz.getNumber());  //Wird zu dieser einen Sitznummer  ein Komma hinzugefügt außerdem werden ein "Leerzeichen" und die weitere Sitznummer hinzugefügt.
                }
            }

            Program.csvPlatz = stringBuilder.ToString();                            //der eben "gebaute" string wird in der Variable "csvPlatz" gespeichert.

            string output=("Uhrzeit: "+ Program.csvUhrzeit);                        //Speichern der Uhrzeit in string
            string output1 = ("Saal: " + Program.csvSaal);                          //Speichern des Saals in string
            string output2 = ("Plätze: " + Program.csvPlatz);                       //Speichern des/der Sitzplätze in string
            string output3 = ("Kosten: " + price + " Euro.");                       //Speichern des zu zahlenden Preises in der string
            sw.WriteLine("Reservierung!");                                          //Speichern in Textdatei            
            sw.WriteLine(output);                                                   //Variable "output" in Textdatei
            sw.WriteLine("{0}.         {1}.",output1,output2);                      //Speichern der Variablen "output1" und "output2" in Textdatei
            sw.WriteLine(output3);                                                  //speichern der Variable "output3" in Textdatei
            sw.Close();                                                             //Beenden der StreamWriter-Methode.


        }

        public void setPriceLocation()                                                          //Position und "Layout" des Preis-Schriftzugs.
        {
            int x = this.Width;                                                                 //gleichsetzen der Variable "x" mit "Width"
            int y = this.Height;                                                                //gleichsetzen der Variable "y" mit "Height"
            y -= 100;                                                                           //y wird "-100" gerechnet (Verschiebt den Text auf der y-Achse
            x -= (this.Width - 16);                                                             //x wird -"Widh-70" gerechnet (Verschiebt den Text auf der x-Achse

          
            premiumPriceLabel.Text = "Preis: " + Sitzplatz.normalPrice + "€";                           //Der Text des ersten "Labels" wird auf "Preis" gesetzt, dahinter wird der Preis eingefügt.
            priceLabel.Text = "Premium  Preis: " + Sitzplatz.premiumPrice + "€";                        //Der Text des zweite "Label" wird auf "Premium Preis" gesetzt, dies sind die ersten 5 Reihen.
                                                        
            costLabel.Location = new Point(x, y);                                                       //Position des Labels für den Gesamtpreis.
            costLabel.Name = "Preis";                                                                   //"Name" des Labels
            costLabel.Font = new Font("Calibri", 20, FontStyle.Underline);                              //"Aussehen" des Textes
            costLabel.Text = "Gesamtpreis: 0 €";                                                        //"Inhalt"/"Text" des Labeles.
            costLabel.Update();                                                                         //Aktualisieren
            this.Update();                                                                              //Aktualisieren v2
        }

        public void changePriceLabel()
        { 
            costLabel.Text = "Gesamtpreis: " + price + " €";
            costLabel.Update();
            this.Update();
        }

        public void addSitzplaetze()
        {


            int x = 20;             //"Startposition" der Knöpfe/Buttons, veränderbar
            int y = 50;             //^


            int buttonsInRow = 0;   // Wie viele Knöpfe momentan in der momentanen Reihe sind



            for (int i = 0; i < sitzplaetze.Count; i++)
            {
                Sitzplatz sitzplatz = sitzplaetze.ElementAt(i);


                int number = i + 1;


                Button button = new Button();   // erstellen des "Knopfes"
                button.Width = 35;              // Größes des Knopfes
                button.Height = 35;             // ^
                this.Controls.Add(button);      // Knopf zur Form hinzufügen
                button.Text = number + "";      // Text des Buttons ändern.
                button.Location = new Point(x, y); // Button auf die richtige Position in der Form setzten.

                button.Click += new EventHandler(seatButtonClickEvent); // Event hinzufügen das beim Klicken auf dem Button ausgeführt wird.
                button.BackColor = sitzplatz.istVerfuegbar() ? (sitzplatz.istPremium() ? Color.DarkGreen : Color.Green) : Color.Red; // Button farbe
                button.Show(); // anzeigen des buttons

                buttonsInRow++; 


                if (angaben[1] / 2 == buttonsInRow)
                {
                    x += 95;
                }
                else if (angaben[1] == buttonsInRow)
                {
                    x = 20;
                    y += 45;
                    buttonsInRow = 0;
                }
                else
                {
                    x += 45;
                }
            }
            Console.WriteLine("Alle Sitze hinzugefügt!");
            this.Update();
        }

        protected void seatButtonClickEvent(object sender, EventArgs e)
        {
            //Instanzbildung
            int[] sitzNummer = new int[sitzplaetze.Count];

            Button button = sender as Button;
            
            Console.WriteLine("Sitz ausgewählt: " + button.Text);

            Sitzplatz seat = sitzplaetze.ElementAt((int.Parse(button.Text) - 1));
           
       
            
                
            
           
            seat.setVerfuegbar(!seat.istVerfuegbar());
            button.BackColor = seat.istVerfuegbar() ? (seat.istPremium() ? Color.DarkGreen : Color.Green) : Color.DimGray; // IF else in einer Zeile. man könnte auch if(abfrage){ dann }else{sonst} benutzten


            if (!seat.istVerfuegbar()) // Ausrufezeichen ist if(boolean == false) ohne Ausrufenzeichen wäre es if(boolean == true)
            { 

                price += seat.istPremium() ? Sitzplatz.premiumPrice : Sitzplatz.normalPrice; // IF else in einer Zeile. man könnte auch if(abfrage){ dann }else{sonst} benutzten - Wenn der Sitz kein "Premiumsitzt" ist wird der normale Preis gezahlt, wenn doch wird der Premium Preis bezahlt

            }
            else
            {
                price -= seat.istPremium() ? Sitzplatz.premiumPrice : Sitzplatz.normalPrice; // IF else in einer Zeile. man könnte auch if(abfrage){ dann }else{sonst} benutzten
            }

            if(price < 0)
            {
             //   price = 0;
            }
            changePriceLabel();

            // set price

            this.Update();

        }

        private int[] calculateFormSize()
        {
            int[] size = new int[2];


            int seatWidt = 35; //   seat widt
            int seatGap = 10;  // seat gap
            int seatHeight = 35; // seat height



            // Form size
            // we´re starting with the widt

            int corridorWidt = 50; // size of the corridor widt between the seats


            int widt = 40 /* size @ edges */ + (seatWidt * angaben[1]) + (seatGap * angaben[1]) + corridorWidt + 20;
            int height = 100 /* size @ edges */ + (seatHeight * angaben[0]) + (seatGap * angaben[0]) + 90;

            Console.WriteLine("widt: " + widt);
            Console.WriteLine("height: " + height);

            size[0] = widt;
            size[1] = height;
            return size;
        }


        /*private void button1_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }*/
    }
}
