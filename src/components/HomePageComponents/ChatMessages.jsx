import React, { useState, useEffect, useContext, useRef } from 'react'
import { collection, doc, deleteDoc, addDoc, where } from 'firebase/firestore'
import { Context } from '../../index'
import { useAuthState } from 'react-firebase-hooks/auth'
import MessageElement from './MessageElement'

export default function ChatMessages({ dialog, onEmptyDelete, onAddMessage, onDeleteMessages, onShowDialogs }) {
    const [messages, setMessages] = useState([])
    const textFieldRef = useRef()
    const { db, auth } = useContext(Context)
    const [user] = useAuthState(auth)
    const messagesCollectionRef = collection(db, 'messages')

    useEffect(async () => {
        setMessages(dialog?.messages ?? [])
    }, [])

    const submitDeleteMessage = async (event) => {
        var selectedMessages = messages.filter((msg) => msg.isSelected)
        if (selectedMessages.length == 0) {
            onEmptyDelete()
        }
        for (var i = 0; i < selectedMessages.length; i++) {
            var message = selectedMessages[i]
            const messageDoc = doc(db, "messages", message.id)
            await deleteDoc(messageDoc)
        }
        setMessages(messages.filter((msg) => !msg.isSelected))
        onDeleteMessages(selectedMessages)
    }

    const submitSendMessage = async (event) => {
        if (textFieldRef.current.value === '') return
        var timestamp = (new Date()).getTime();
        var message = { text: textFieldRef.current.value, authorName: user.email, date: timestamp, dialogId: dialog.id }
        let ref = await addDoc(messagesCollectionRef, message)
        message.id = ref.id
        setMessages((prevMessages) => [...prevMessages, message])
        onAddMessage(message)
        textFieldRef.current.value = null
    }

    const onMessageClick = (message) => {
        message.isSelected = !message.isSelected;
        setMessages((prevMessages) => { return [...messages] })
    }

    return (
        <section className='chat'>
            <div className="chat--header">
                <button id="display-dialogs-button" className="display-dialogs-button" onClick={onShowDialogs}>~</button>
                <p id="current-user">{dialog?.secondUsername}</p>
                <button id="delete-button" className="delete-button" onClick={submitDeleteMessage}>Delete</button>
            </div>
            <ul id="chat--messages">
                {
                    messages.map((message) =>
                        <MessageElement key={message.id} mainUsername={user.email} message={message} onClick={onMessageClick} />
                    )
                }
            </ul>
            <form name="message" className="chat--footer-textarea">
                <textarea name="text" className="message-text" ref={textFieldRef} maxLength="40" rows="3" placeholder="Write a message..."></textarea>
                <input type="button" id="button-send" value="Send" onClick={submitSendMessage} />
            </form>
        </section>
    )
}
