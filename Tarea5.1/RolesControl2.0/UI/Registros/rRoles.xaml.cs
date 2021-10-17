using RolesControl2._0.BLL;
using RolesControl2._0.DAL;
using RolesControl2._0.Entidades;
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
using System.Windows.Shapes;

namespace RolesControl2._0.UI.Registros
{
    /// <summary>
    /// Interaction logic for rRoles.xaml
    /// </summary>
    public partial class rRoles : Window
    {
        private Roles rol = new Roles();
        private Permisos permiso = new Permisos();

        public decimal total = 0;
        public int cantidad0 = 0;
        public int cantidad1 = 0;
        public int cantidad2;
        public int cantidad3;

        public rRoles()
        {
            InitializeComponent();
            this.DataContext = rol;

            PermisosComboBox.ItemsSource = PermisosBLL.GetPermisos();
            PermisosComboBox.SelectedValuePath = "PermisoId";
            PermisosComboBox.DisplayMemberPath = "Nombre";
        }
        private void Cargar()
        {
            this.DataContext = null;
            this.DataContext = rol;
        }
        private void Limpiar()
        {
            this.rol = new Roles();
            this.DataContext = rol;
        }
        private bool Validar()
        {
            bool esValido = true;
            if (PermisosComboBox.Text.Length == 0)
            {
                esValido = false;
                MessageBox.Show("Ha ocurrido un error, Inserte el permiso", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return esValido;
        }
        private bool ValidarGuardar()
        {
            bool esValido = true;
            if (DetalleDataGrid.Items.Count == 0)
            {
                esValido = false;
                MessageBox.Show("Ha ocurrido un error, Debe agregar roles", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return esValido;
        }
        private bool ExisteEnLaBaseDeDatos()
        {
            Roles esValido = RolesBLL.Buscar(rol.RolId);

            return (esValido != null);
        }
        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            Roles encontrado = RolesBLL.Buscar(rol.RolId);

            if (encontrado != null)
            {
                rol = encontrado;
                Cargar();
                int totalp = RolesBLL.Total(rol);
                string TotalPermiso = totalp.ToString();
                TotalTextBlock.Text = TotalPermiso;
            }
            else
            {
                Limpiar();
                MessageBox.Show("Rol no existe en la base de datos", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarGuardar())
                return;
            bool paso = false;

            if (rol.RolId == 0)
            {
                paso = RolesBLL.Guardar(rol);
            }
            else
            {
                if (ExisteEnLaBaseDeDatos())
                {
                    paso = RolesBLL.Guardar(rol);
                }
                else
                {
                    MessageBox.Show("No existe en la base de datos", "ERROR");
                }
            }

            if (paso)
            {
                Limpiar();
                MessageBox.Show("Guardado!", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Fallo al guardar", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            Roles existe = RolesBLL.Buscar(rol.RolId);

            if (existe == null)
            {
                MessageBox.Show("No existe la tarea en la base de datos", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                RolesBLL.Eliminar(rol.RolId);
                MessageBox.Show("Eliminado", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();
            }
        }

        private void AgregarFilaButton_Click(object sender, RoutedEventArgs e)
        {
            Contexto contexto = new Contexto();

            if (!Validar())
                return;
            rol.RolesDetalle.Add(new RolesDetalle(rol.RolId, (int)PermisosComboBox.SelectedValue, PermisosComboBox.Text.ToString() ,ActivoCheckBox.IsEnabled));

            Cargar();
            int totalp = RolesBLL.Total(rol);
            string TotalPermiso = totalp.ToString();

            TotalTextBlock.Text = TotalPermiso;
        }

        private void RemoverFilaButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetalleDataGrid.Items.Count >= 1 && DetalleDataGrid.SelectedIndex <= DetalleDataGrid.Items.Count - 1)
            {
                rol.RolesDetalle.RemoveAt(DetalleDataGrid.SelectedIndex);
                Cargar();
                
            }
        }
        public void TotalSum()
        {
            if (PermisosComboBox.SelectedIndex == 0)
            {
                total++;
                cantidad0++;
            }
            else if (PermisosComboBox.SelectedIndex == 1)
            {
                total++;
                cantidad1++;
            }
            else if (PermisosComboBox.SelectedIndex == 2)
            {
                total++;
                cantidad2++;
            }
            else if (PermisosComboBox.SelectedIndex == 3)
            {
                total++;
                cantidad3++;
            }
        }
    }
}
