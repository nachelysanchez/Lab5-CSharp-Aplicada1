using RolesControl2._0.BLL;
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

        public int total = 0;
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
            this.permiso = new Permisos();
            this.DataContext = permiso;
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
            Permisos pencontrado = PermisosBLL.Buscar(permiso.PermisoId);

            if (encontrado != null)
            {
                rol = encontrado;
                permiso = pencontrado;
                Cargar();
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
            if (!Validar())
                return;
            rol.RolesDetalle.Add(new RolesDetalle(rol.RolId, (int)PermisosComboBox.SelectedValue, PermisosComboBox.Text.ToString() ,ActivoCheckBox.IsEnabled));

            Cargar();
            TotalSum();
            string TotalPermiso = total.ToString();
            permiso.VecesAsignado = total;
            string permiso0 = cantidad0.ToString();
            string permiso1 = cantidad1.ToString();
            string permiso2 = cantidad2.ToString();
            string permiso3 = cantidad3.ToString();

            TotalTextBlock.Text = TotalPermiso;
            P0TextBlock.Text = permiso0;
            P1TextBlock.Text = permiso1;
            P2TextBlock.Text = permiso2;
            P3TextBlock.Text = permiso3;
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
        //private int SumaPermisos()
        //{
        //    int[] permisos = { 0, 1, 2, 3 };
        //    if (PermisosComboBox.SelectedIndex == permisos[0])
        //    {
        //        return (Cantidad0++);
        //    }
        //    else if (PermisosComboBox.SelectedIndex == permisos[1])
        //    {
        //        return (Cantidad1++);
        //    }
        //    else if (PermisosComboBox.SelectedIndex == permisos[2])
        //    {
        //        return (Cantidad2++);
        //    }
        //    else if (PermisosComboBox.SelectedIndex == permisos[3])
        //    {
        //        return (Cantidad3++);
        //    }
        //    return 0;
        //}
    }
}
