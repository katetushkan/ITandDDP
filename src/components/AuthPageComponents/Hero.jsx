import React, { useState, useContext } from "react";
import { signInWithEmailAndPassword } from "firebase/auth"
import { Context } from "../../index";

export default function Hero() {
    const { auth } = useContext(Context)
    const [user, setUser] = useState({})

    const handleSubmit = async (event) => {
        event.preventDefault()
        if (user.email === '' || user.password === '') {
            return
        }
        try {
            await signInWithEmailAndPassword(auth, user.email, user.password)
        } catch (e) {
            alert(`incorrect login/password ${e}`)
        }
    }

    const handleChange = (event) => {
        setUser(user => ({ ...user, [event.target.name]: event.target.value }))
    }

    return (
        <section className="hero">
            <h1>My awesome lab</h1>
            <p>Enter nickname to start chatting</p>
            <form className="login-form" onSubmit={handleSubmit}>
                <input id="login-textarea" name="email" type="text" onChange={handleChange} maxLength="18" placeholder="Big Bob" />
                <input id="password-textarea" name="password" type="password" onChange={handleChange} maxLength="18" placeholder="******" />
                <input id="login-button" className="login-button" type="submit" value="Start Chatting" />
            </form>
        </section>
    );
}
