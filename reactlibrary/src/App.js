import React, { useState } from "react";
import BookCreateForm from "./Components/BookCreateForm";
import BookUpdateForm from "./Components/BookUpdateForm";

export default function App() {
  const [books, setBooks] = useState([]);
  const [showingCreateNewBookForm, setShowingCreateNewBookForm] = useState(false);
  const [bookCurrentlyBeingUpdated, setBookCurrentlyBeingUpdated] = useState(null);

  function getBooks() {
    const url = "https://localhost:7175/api/books/";

    fetch(url, {
      method: "GET",
    })
      .then((response) => response.json())
      .then((booksFromServer) => {
        console.log(booksFromServer);
        setBooks(booksFromServer);
      })
      .catch((error) => {
        console.log(error);
        alert(error);
      });
  }
  function deleteBook(bookId) {
    const url = `https://localhost:7175/api/books/${bookId}`;
  
    fetch(url, {
      method: "DELETE",
    })
      .then((response) => response.json())
      .then((responseFromServer) => { // Added parentheses here
        console.log(responseFromServer);
        onBookDeleted(bookId);
      })
      .catch((error) => {
        console.log(error);
        alert(error);
      });
  }
  
  return (
    <div className="container">
      <div className="row min-vh-100">
        <div className="col d-flex flex-column justify-content-center align-items-center">
          {(showingCreateNewBookForm === false && bookCurrentlyBeingUpdated === null) && (
            <div>
              <h1>
                Books <i className="fa fa-book"></i>
              </h1>
              <div className="mt-5">
                <button
                  onClick={getBooks}
                  className="btn btn-dark btn-lg w-100"
                >
                  All Books
                </button>
                <button
                  onClick={() => setShowingCreateNewBookForm(true)}
                  className="btn btn-primary btn-lg w-100 mt-4"
                >
                  Add new book
                </button>{" "}
              </div>
            </div>
          )}

          {(books.isSuccess &&
            showingCreateNewBookForm === false && bookCurrentlyBeingUpdated === null) &&
            renderBooksTable()}
          {showingCreateNewBookForm && (
            <BookCreateForm onBookCreated={onBookCreated} />
          )}
          {bookCurrentlyBeingUpdated !== null && <BookUpdateForm book={bookCurrentlyBeingUpdated} onBookUpdated={onBookUpdated}/>}
        </div>
      </div>
    </div>
  );

  function renderBooksTable() {
    return (
      <div className="table-responsive mt-5">
        <table className="table table-bordered border-dark">
          <thead>
            <tr>
              <th scope="col">Title</th>
              <th scope="col">Author</th>
              <th scope="col">PublishedOn</th>
              <th scope="col">Genre</th>
              <th scope="col">Description</th>
              <th scope="col">Availability</th>
            </tr>
          </thead>
          <tbody>
            {books.result.map((book) => (
              <tr key={book.id}>
                <td>{book.title}</td>
                <td>{book.author}</td>
                <td>{book.publishedOn}</td>
                <td>{book.genre}</td>
                <td>{book.description}</td>
                <td>
                  {book.availability && (
                    <input type="checkbox" checked="checked" disabled />
                  )}
                  {book.availability === false && (
                    <input type="checkbox" disabled />
                  )}
                </td>
                <td>
                  <button onClick={() => setBookCurrentlyBeingUpdated(book)} className="btn btn-dark btn-lg mx-3 my-3">Update</button>
                  <button onClick={() => {if(window.confirm(`Are you sure you want to delete the book titled "${book.title}"?`)) deleteBook(book.bookId)}}className="btn btn-secondary btn-lg">Delete</button>                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  }

  function onBookCreated(createdBook) {
    setShowingCreateNewBookForm(false);

    if (createdBook === null) {
      return;
    }

    getBooks();
  }
  function onBookUpdated(updatedBook){
    setBookCurrentlyBeingUpdated(null);

    if(updatedBook === null){
      return;
    }

    let booksCopy = [...books.result];

    const index = booksCopy.findIndex((booksCopyBook, currentIndex) => {
      if (booksCopyBook.bookId === updatedBook.bookId) {
        return true;
      }
    });

    if (index !== -1) {
      booksCopy[index] = updatedBook;
    }

    setBooks(booksCopy);
    
    getBooks();
  }
  function onBookDeleted(deletedBookId){
  
    let booksCopy = [...books.result];

    const index = booksCopy.findIndex((booksCopyBook, currentIndex) => {
      if (booksCopyBook.bookId === deletedBookId) {
        return true;
      }
    });

    if (index !== -1) {
      booksCopy.splice(index, 1);
    }

    setBooks(booksCopy);
    
    getBooks();

    alert('Book successfully deleted.')
  }
  
}
