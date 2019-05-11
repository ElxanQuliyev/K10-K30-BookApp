using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookApp1.Model;

namespace BookApp1
{
    public partial class Form1 : Form
    {
        NewBookEntities db = new NewBookEntities();
        Book selectedBook;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddBookBtn();
            cmbAuthor.Items.AddRange(db.Authors.Select(a => a.FullName).ToArray());
            LoadBook();
        }
        private void LoadBook()
        {
            dtgView.DataSource = db.Books.Select(a =>
                new
                {
                    id=a.Id,
                    Kitab_Adi = a.Name,
                    Sayi = a.Count,
                    Muellif = a.Author.FullName,
                  
                }
                ).ToList();

        }

     
        private void CheckText()
        {
            foreach(Control cb in this.Controls)
            {
                if(cb is TextBox || cb is ComboBox )
                {
                    cb.Text = "";
                }
            }
        }
        private void BtnAddBook_Click(object sender, EventArgs e)
        {
            string bookName = txtBookName.Text;
            string bookCount = txtCount.Text;
            string author = cmbAuthor.Text;
            int bookCounts;
            if (bookName != String.Empty &&
                bookCount != String.Empty &&
                author != String.Empty &&
                int.TryParse(bookCount, out bookCounts)
                )
            {
                int authorId = db.Authors.First(a => a.FullName == author).Id;
                Book book = new Book();
                book.Name = bookName;
                book.Count = bookCounts;
                book.author_id = authorId;
                db.Books.Add(book);
                db.SaveChanges();
                CheckText();
                LoadBook();
                MessageBox.Show("Kitab ugurla əlavə olundu", "Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Zehmet olmasa butun xanlari doldurun","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void AddBookBtn()
        {
            btnAddBook.Visible = true;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
        }
        private void DeleteEditBtn()
        {
            btnAddBook.Visible = false;
            btnDelete.Visible = true;
            btnEdit.Visible = true;
        }

        private void DtgView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           int bookId= (int)dtgView.Rows[e.RowIndex].Cells[0].Value;
            selectedBook = db.Books.Find(bookId);
            txtBookName.Text = selectedBook.Name;
            txtCount.Text = selectedBook.Count.ToString();
            cmbAuthor.Text = selectedBook.Author.FullName;
            DeleteEditBtn();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBook != null)
            {

               DialogResult result= MessageBox.Show("Qaqa Silim?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)

                    MessageBox.Show("Qaqa Sildim", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                db.Books.Remove(selectedBook);

                    db.SaveChanges();

                    CheckText();
                    LoadBook();
                    AddBookBtn();
                }
               
               

            }
        }
}
