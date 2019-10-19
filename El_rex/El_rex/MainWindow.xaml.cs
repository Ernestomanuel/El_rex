using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace El_rex
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Dinosaurios.mdb";
            MostrarDatos();
        }

        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Dinos";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtNadop.Text = "";
            txtNombre.Text = "";
            cbTipo.SelectedIndex = 0;
            cbDino.SelectedIndex = 0;
            cbGenero.SelectedIndex = 0;
            cbColor.SelectedIndex = 0;
            txtTelefono.Text = "";
            btnNuevo.Content = "Nuevo";
            txtNadop.IsEnabled = true;
        }
        //Editamos alumnos existentes
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtNadop.Text = row["Numero_adopt"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                cbTipo.Text = row["Tipo"].ToString();
                cbDino.Text = row["Dino"].ToString();
                cbGenero.Text = row["Genero"].ToString();
                cbColor.Text = row["Color"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtNadop.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un alumno de la lista...");
            }
        }

        //Eliminamos Alumnos
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Tenis where Numero_adopt=" + row["Numero_adopt"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Adopcion Eliminado correctamenta.");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un alumno de la lista...");
            }
        }
        //Salimos de la aplicación
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Agregamos nuevos alumnos
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtNadop.Text != "")
            {
                if (txtNadop.IsEnabled == true)
                {
                    if (cbTipo.Text != "Selecciona el tipo de dino")
                    {
                        if (cbTipo.IsEnabled == true)
                        {
                            if (cbGenero.Text != "Genero")
                            {
                                if (cbGenero.IsEnabled == true)
                                {
                                    if (cbColor.Text != "Color")
                                    {
                                        if (cbColor.IsEnabled == true)
                                        {
                                            cmd.CommandText = "insert into Dinos(Numero_adopt,Nombre,Tipo,Dino,Genero,Color,Telefono) " +
                            "Values(" + txtNadop.Text + ",'" + txtNombre.Text + "','" + cbTipo.Text + "', '" + cbDino.Text + "', '" + cbGenero.Text + "', '" + cbColor.Text + "', " + txtTelefono.Text + ")";
                                            cmd.ExecuteNonQuery();
                                            MostrarDatos();
                                            MessageBox.Show("Adopcion agregado correctamente.");
                                            LimpiaTodo();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Selecciona el color.");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Selecciona el genero.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selecciona tipo de Dino.");
                    }
                }
                else
                {
                    cmd.CommandText = "update Tenis set Nombre='" + txtNombre.Text + "',Tipo='" + cbTipo.Text + "',Dino='" + cbDino.Text + "',Genero='"
                        + cbGenero.Text + "',Color='" + cbColor.Text + "', Telefono=" + txtTelefono.Text + " where Numero_adopt=" + txtNadop.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos de adopcion Actualizados.");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el numero de adopcion.");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }
    }
}

