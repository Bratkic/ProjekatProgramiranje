using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EDnevnik
{
    public partial class Ocena : Form
    {
        DataTable dt_ocena;
        public Ocena()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Ocena_Load(object sender, EventArgs e)
        {
            cmb_GodinaPopulate();
            cmb_Predmet.Enabled = false;
            cmb_Odeljenje.Enabled = false;
            cmb_Ucenik.Enabled = false;
            cmb_Ocena.Items.Add(1);
            cmb_Ocena.Items.Add(2);
            cmb_Ocena.Items.Add(3);
            cmb_Ocena.Items.Add(4);
            cmb_Ocena.Items.Add(5);
            cmb_Ocena.Enabled = false;
            cmb_ProfesorPopulate();
        }
        private void cmb_GodinaPopulate()
        {
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM skolska_godina", veza);
            DataTable dt_godina = new DataTable();
            adapter.Fill(dt_godina);
            cmb_Godina.DataSource = dt_godina;
            cmb_Godina.ValueMember = "id";
            cmb_Godina.DisplayMember = "naziv";
            cmb_Godina.SelectedValue = 2;
        }
        private void cmb_Godina_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Godina.IsHandleCreated && cmb_Godina.Focused)
            {
                cmb_ProfesorPopulate();
                
            }
        }
        private void cmb_ProfesorPopulate()
        {
           
            StringBuilder naredba = new StringBuilder("Select distinct osoba.id as id, ime+' '+prezime as naziv from osoba");
            naredba.Append(" JOIN raspodela on osoba.id=nastavnik_id");
            naredba.Append(" where godina_id = " + cmb_Godina.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_profesor = new DataTable();
            adapter.Fill(dt_profesor);
            cmb_Profesor.DataSource = dt_profesor;
            cmb_Profesor.ValueMember = "id";
            cmb_Profesor.DisplayMember = "naziv";
            cmb_Profesor.SelectedIndex = -1;

           
        }

        private void cmb_Profesor_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Profesor.IsHandleCreated && cmb_Profesor.Focused)
            {
                cmb_PredmetPopulate();
                cmb_Predmet.Enabled = true;
                
                cmb_Odeljenje.SelectedIndex = -1;
                cmb_Odeljenje.Enabled = false;

                cmb_Ucenik.SelectedIndex = -1;
                cmb_Ucenik.Enabled = false;

                cmb_Ocena.SelectedIndex = -1;
                cmb_Ocena.Enabled = false;

                dt_ocena = new DataTable();
               
                Grid_Ocene.DataSource = dt_ocena;

            }
        }
        private void cmb_PredmetPopulate()
        {
            
            StringBuilder naredba = new StringBuilder("select distinct predmet.id as id, naziv from predmet ");
            naredba.Append(" join raspodela on predmet.id = predmet_id ");
            naredba.Append(" where godina_id= " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_predmet = new DataTable();
            adapter.Fill(dt_predmet);
            cmb_Predmet.DataSource = dt_predmet;
            cmb_Predmet.ValueMember = "id";
            cmb_Predmet.DisplayMember = "naziv";
            cmb_Predmet.SelectedIndex = -1;




        }

        private void cmb_Predmet_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Predmet.IsHandleCreated && cmb_Predmet.Focused)
            {
                cmb_OdeljenjePopulate();
                cmb_Odeljenje.Enabled = true;
                cmb_Odeljenje.SelectedIndex = -1;

                cmb_Ucenik.SelectedIndex = -1;
                cmb_Ucenik.Enabled = false;

                cmb_Ocena.SelectedIndex = -1;
                cmb_Ocena.Enabled = false;

                dt_ocena = new DataTable();
                Grid_Ocene.DataSource = dt_ocena;
            }
        }

        private void cmb_OdeljenjePopulate()
        {
            StringBuilder naredba = new StringBuilder("Select distinct odeljenje.id as id, str(razred)+ '-' + indeks as naziv from odeljenje ");
            naredba.Append(" join raspodela on odeljenje.id = odeljenje_id ");
            naredba.Append(" where raspodela.godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            naredba.Append(" and predmet_id = " + cmb_Predmet.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_odeljenje = new DataTable();
            adapter.Fill(dt_odeljenje);
            cmb_Odeljenje.DataSource = dt_odeljenje;
            cmb_Odeljenje.ValueMember = "id";
            cmb_Odeljenje.DisplayMember = "naziv";

        }

        private void cmb_Odeljenje_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_Odeljenje.IsHandleCreated && cmb_Odeljenje.Focused)
            {
                cmb_UcenikPopulate();
                cmb_Ucenik.Enabled = true;
                cmb_Ocena.Enabled = true;
                GridPopulate();
                UcenikOcenaIdSet(0);
            }
            
        }
        private void cmb_UcenikPopulate()
        {
            StringBuilder naredba = new StringBuilder("Select osoba.id as id, ime + ' ' + prezime as naziv from osoba ");
            naredba.Append(" join upisnica on osoba.id = osoba_id ");
            naredba.Append(" where upisnica.odeljenje_id = " + cmb_Odeljenje.SelectedValue.ToString());
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            DataTable dt_ucenik = new DataTable();
            adapter.Fill(dt_ucenik);
            cmb_Ucenik.DataSource=dt_ucenik;
            cmb_Ucenik.ValueMember = "id";
            cmb_Ucenik.DisplayMember = "naziv";
            cmb_Ucenik.SelectedIndex = -1;

        }
        private void GridPopulate()
        {
            StringBuilder naredba = new StringBuilder("Select ocena.id as id, ime+ ' ' + prezime as naziv, ocena, ucenik_id, datum from osoba");
            naredba.Append(" join ocena on osoba.id=ucenik_id ");
            naredba.Append(" join raspodela on raspodela_id = raspodela.id");
            naredba.Append(" where raspodela_id = " );
            naredba.Append(" (select id from raspodela ");
            naredba.Append(" WHERE godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            naredba.Append(" and predmet_id = " + cmb_Predmet.SelectedValue.ToString());
            naredba.Append(" and odeljenje_id = " + cmb_Odeljenje.SelectedValue.ToString() + ")");
            //textBox2.Text = naredba.ToString();
            SqlConnection veza = Konekcija.Connect();
            SqlDataAdapter adapter = new SqlDataAdapter(naredba.ToString(), veza);
            dt_ocena = new DataTable();
            adapter.Fill(dt_ocena);
            Grid_Ocene.DataSource = dt_ocena;
            Grid_Ocene.AllowUserToAddRows = false;
            Grid_Ocene.Columns["ucenik_id"].Visible = false;

        }

        private void Grid_Ocene_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0)
            {
                UcenikOcenaIdSet(e.RowIndex);

            }
        }
        private void UcenikOcenaIdSet(int broj_sloga)
        {
            cmb_Ucenik.SelectedValue = dt_ocena.Rows[broj_sloga]["ucenik_id"];
            cmb_Ocena.SelectedItem = dt_ocena.Rows[broj_sloga]["ocena"];
            txt_id.Text = dt_ocena.Rows[broj_sloga]["id"].ToString();
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            //insert
            StringBuilder naredba = new StringBuilder("select id from raspodela ");
            naredba.Append(" where godina_id = " + cmb_Godina.SelectedValue.ToString());
            naredba.Append(" and nastavnik_id = " + cmb_Profesor.SelectedValue.ToString());
            naredba.Append(" and predmet_id = " + cmb_Predmet.SelectedValue.ToString());
            naredba.Append(" and odeljenje_id = " + cmb_Odeljenje.SelectedValue.ToString());

            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza);
            int id_raspodele = 0;
            try
            {
                veza.Open();
                id_raspodele = (int)komanda.ExecuteScalar();
                veza.Close();
            }
            catch (Exception Greska)
            {
                MessageBox.Show(Greska.Message);
                
            }
            if (id_raspodele>0)
            {
                naredba = new StringBuilder("insert into ocena (datum, raspodela_id, ucenik_id, ocena) values ('");
                DateTime datum = Datum.Value;
                naredba.Append(datum.ToString("yyyy-MM-dd") + "', '");
                naredba.Append(id_raspodele.ToString() + "', '");
                naredba.Append(cmb_Ucenik.SelectedValue.ToString() + "', '");
                naredba.Append(cmb_Ocena.SelectedItem.ToString() + "')");
                komanda = new SqlCommand(naredba.ToString(), veza);
                try
                {
                    veza.Open();
                    komanda.ExecuteNonQuery();
                    veza.Close();
                }
                catch (Exception Greska)
                {
                    MessageBox.Show(Greska.Message);
                    throw;
                }
                GridPopulate();
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            //UPDATE
            if (Convert.ToInt32(txt_id.Text)>0)
            {
                DateTime datum = Datum.Value;
                StringBuilder naredba = new StringBuilder("update ocena set ");
                naredba.Append(" ucenik_id = '" + cmb_Ucenik.SelectedValue.ToString() + "' , ");
                naredba.Append(" ocena = '" + cmb_Ocena.SelectedItem.ToString() + "' , ");
                naredba.Append(" datum = '" + datum.ToString("yyyy-MM-dd") + "' ");
                naredba.Append(" where id = " + txt_id.Text);
                SqlConnection veza = Konekcija.Connect();
                SqlCommand Komanda = new SqlCommand(naredba.ToString(), veza);
                try
                {
                    veza.Open();
                    Komanda.ExecuteNonQuery();
                    veza.Close();
                }
                catch (Exception Greska)
                {

                    MessageBox.Show(Greska.Message);
                }
                GridPopulate();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            // DELETE

            if (Convert.ToInt32(txt_id.Text)>0)
            {
                string naredba = "delete from ocena where id = " + txt_id.Text;
                SqlConnection veza = Konekcija.Connect();
                SqlCommand Komanda = new SqlCommand(naredba, veza);
                try
                {
                    veza.Open();
                    Komanda.ExecuteNonQuery();
                    veza.Close();
                    GridPopulate();
                    UcenikOcenaIdSet(0);
                }
                catch (Exception Greska)
                {
                    MessageBox.Show(Greska.Message);
                    
                }

            }
        }
    }
}
