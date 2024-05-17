##### This README provides detailed information on how to interact with the API, including request and response examples for each endpoint.

# Question and Answer API

This API provides endpoints to manage questions and submit answers.

## Question API

### Create a New Question

Create a new question.

- **URL:** `/api/question`
- **Method:** `POST`
- **Request Body:**
  ```json
  {
      "type": "Dropdown",
      "description": "What is your favorite color?",
      "options": ["Red", "Blue", "Green"]
  }
  ```
- **Response:**
  - **Code:** `201 Created`
  - **Content:** 
    ```json
    {
        "id": "question_id",
        "type": "Dropdown",
        "description": "What is your favorite color?",
        "options": ["Red", "Blue", "Green"]
    }
    ```

### Update an Existing Question

Update an existing question.

- **URL:** `/api/question/{id}`
- **Method:** `PUT`
- **Path Parameters:**
  - `id`: The ID of the question to update
- **Request Body:**
  ```json
  {
      "type": "Dropdown",
      "description": "What is your favorite color?",
      "options": ["Red", "Blue", "Green"]
  }
  ```
- **Response:**
  - **Code:** `204 No Content`

### Get a Question

Get a question by its ID.

- **URL:** `/api/question/{id}`
- **Method:** `GET`
- **Path Parameters:**
  - `id`: The ID of the question to retrieve
- **Response:**
  - **Code:** `200 OK`
  - **Content:** 
    ```json
    {
        "id": "question_id",
        "type": "Dropdown",
        "description": "What is your favorite color?",
        "options": ["Red", "Blue", "Green"]
    }
    ```

### Get All Questions

Get all questions.

- **URL:** `/api/question`
- **Method:** `GET`
- **Response:**
  - **Code:** `200 OK`
  - **Content:** 
    ```json
    [
        {
            "id": "question_id",
            "type": "Dropdown",
            "description": "What is your favorite color?",
            "options": ["Red", "Blue", "Green"]
        },
        ...
    ]
    ```

### Delete a Question

Delete a question by its ID.

- **URL:** `/api/question/{id}`
- **Method:** `DELETE`
- **Path Parameters:**
  - `id`: The ID of the question to delete
- **Response:**
  - **Code:** `200 OK`

## Answer API

### Submit Answers

Submit answers to questions.

- **URL:** `/api/answer`
- **Method:** `POST`
- **Request Body:**
  ```json
  [
      {
          "questionId": "1",
          "response": "Yes"
      },
      {
          "questionId": "2",
          "response": "Blue"
      }
  ]
  ```
- **Response:**
  - **Code:** `200 OK`
    
