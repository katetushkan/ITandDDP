import React, { createContext } from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from "react-router-dom";
import App from './App';
import { initializeApp } from "firebase/app";
import { getFirestore } from "firebase/firestore"
import { getAuth } from "firebase/auth"
import { getStorage } from 'firebase/storage';

const firebaseConfig = {
  apiKey: "AIzaSyAzwJ4MTuEhOr9EnVCEDDoSR53hXj3IvZU",
  authDomain: "lab6-34e20.firebaseapp.com",
  projectId: "lab6-34e20",
  storageBucket: "lab6-34e20.appspot.com",
  messagingSenderId: "787911737835",
  appId: "1:787911737835:web:4afcf3f16d20d0cab8d9ae"
}

const app = initializeApp(firebaseConfig)
const db = getFirestore(app)
const auth = getAuth(app)
const storage = getStorage(app)

export const Context = createContext(null)

ReactDOM.render(
  <Context.Provider value={{
    app,
    db,
    auth,
    storage
  }}>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </Context.Provider>,
  document.getElementById('root')
);