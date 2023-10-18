import React, { useState } from "react";

export default function BookUpdateForm(props) {
  const initialFormData = Object.freeze({
    title: props.book.title,
    author: props.book.autho,
    publishedOn: props.book.publishedOn,
    genre: props.book.genre,
    description: props.book.description,
    availability: props.book.availability,
  });

  const [formData, setFormData] = useState(initialFormData);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const bookToUpdate = {
      bookId: props.book.bookId,
      title: formData.title,
      author: formData.author,
      publishedOn: formData.publishedOn,
      genre: formData.genre,
      description: formData.genre,
      availability: true,
    };

    const url = "https://localhost:7175/api/books/";

    fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(bookToUpdate),
    })
      .then((response) => response.json())
      .then((responseFromServer) => {
        if (responseFromServer.result === false) {
          alert(`failed to add book`);
          responseFromServer.errorMessages.map((err) => console.log(err));
        }
      })
      .catch((error) => {
        console.log(error);
        alert(error);
      });

    props.onBookUpdated(bookToUpdate);
  };

  return (
    <form className="w-100 px-5">
      <h1 className="mt-5">Update the book titled "{props.book.title}" .</h1>

      <div className="mt-5">
        <label className="h3 form-label">Book Title</label>
        <input
          placeholder={formData.title}
          name="title"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>
      <div className="mt-5">
        <label className="h3 form-label">Book Author</label>
        <input
          placeholder={formData.author}
          name="author"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>
      <div className="mt-5">
        <label className="h3 form-label">Book Release Year</label>
        <input
          value={formData.publishedOn}
          name="publishedOn"
          type="date"
          className="form-control"
          onChange={handleChange}
        />
      </div>

      <div className="mt-5">
        <label className="h3 form-label">Book Genre</label>
        <input
          placeholder={formData.genre}
          name="genre"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>
      <div className="mt-5">
        <label className="h3 form-label">Book Description</label>
        <input
          placeholder={formData.description}
          name="description"
          type="text"
          className="form-control"
          onChange={handleChange}
        />
      </div>
      <div className="mt-5">
        <label className="h3 form-label">Available</label>
        <input
          checked={formData.availability}
          name="availability"
          type="checkbox"
          style={{ width: "35px", height: "25px" }}
          className="form-checkbox"
          onChange={(e) => {
            setFormData({
              ...formData,
              availability: e.target.checked,
            });
          }}
        />
      </div>

      <button
        onClick={handleSubmit}
        className="btn btn-success btn-lg w-100 mt-5"
      >
        Submit
      </button>
      <button
        onClick={() => props.onBookUpdated(null)}
        className="btn btn-danger btn-lg w-100 mt-3"
      >
        Cancel
      </button>
    </form>
  );
}
