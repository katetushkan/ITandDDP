import ChatMessages from '../components/HomePageComponents/ChatMessages'
import React, { useState, useEffect, useContext, useRef } from 'react'
import { getDocs, query } from 'firebase/firestore'
import { collection, doc, deleteDoc, addDoc } from 'firebase/firestore'
import { useAuthState } from 'react-firebase-hooks/auth'
import { Context } from '../index'
import DialogElement from '../components/HomePageComponents/DialogElement'

import '../Home.css';

export default function Home() {
    const [dialogs, setDialogs] = useState([])
    const [currentDialog, setCurrentDialog] = useState([])
    const [isDialogsShown, setIsDialogsShown] = useState([])
    const { db, auth } = useContext(Context)
    const [user] = useAuthState(auth)
    const messagesCollectionRef = collection(db, 'messages')
    const dialogsCollectionRef = collection(db, 'dialogs')
    const dialogRef = useRef()

    useEffect(async () => {
        const qMessages = query(messagesCollectionRef)
        const dataMessages = await getDocs(qMessages)
        var messages = dataMessages.docs.map(doc => ({ ...doc.data(), id: doc.id, isSelected: false }))
            .sort(function (a, b) { return a.date - b.date })
        const qDialogs = query(dialogsCollectionRef)
        const dataDialogs = await getDocs(qDialogs)
        var dialogs = dataDialogs.docs.map(doc => ({ ...doc.data(), id: doc.id }))
            .sort(function (a, b) { return a.lastDate - b.lastDate })
        for (var i = 0; i < dialogs.length; i++) {
            var dialog = dialogs[i]
            dialog.messages = messages.filter((msg) => msg.dialogId === dialog.id)
        }
        if (dialogs.length > 0) {
            setCurrentDialog(dialogs[0])
        }
        setDialogs(dialogs)
    }, [])

    const onDialogClick = (id) => {
        var activeDialog = dialogs.find((e) => e.id === id)
        setCurrentDialog(activeDialog)
    }

    const submitCreateDialog = async (event) => {
        event.preventDefault()
        if (dialogRef.current.value === '') return
        var timestamp = (new Date()).getTime();
        var dialog = {
            secondUsername: dialogRef.current.value,
            firstUsername: user.email,
            lastDate: timestamp,
            messages: [],
        }
        let ref = await addDoc(dialogsCollectionRef, dialog)
        dialog.id = ref.id
        setDialogs((prevDialogs) => [...prevDialogs, dialog])
        setCurrentDialog(dialog)
        dialogRef.current.value = null
    }

    const submitDeleteDialog = async (event) => {
        if (dialogs.length === 0) return
        let messages = currentDialog.messages
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i]
            const messageDoc = doc(db, "messages", message.id)
            await deleteDoc(messageDoc)
        }
        const dialogsDoc = doc(db, "dialogs", currentDialog.id)
        await deleteDoc(dialogsDoc)
        var newDialogs = [...dialogs].filter((dialog) => dialog.id !== currentDialog.id)
        setDialogs(newDialogs)
        setCurrentDialog(newDialogs[0])
    }

    const onAddMessage = (message) => {
        currentDialog.messages.push(message)
        setCurrentDialog(currentDialog)
    }

    const onDeleteMessages = (messages) => {
        let currentMessages = currentDialog.messages;
        for (var i = 0; i < messages.length; i++) {
            var index = currentMessages.indexOf(messages[i])
            currentMessages.splice(index, 1)
        }
        currentDialog.messages = currentMessages
        setCurrentDialog(currentDialog)
    }

    const onShowDialogs = (event) => {
        let state = isDialogsShown
        setIsDialogsShown(!state)
    }

    return (
        <main className='home-container'>
            <ul id="chat-list" key={isDialogsShown} className={"chat-list " + (isDialogsShown ? 'visible' : 'non-visible')}>
                <form name="dialog" className="chat-add-dialog" onSubmit={submitCreateDialog}>
                    <input className="chat-add-dialog--textarea" ref={dialogRef} name="text" maxLength="18" placeholder="Nickname..." />
                    <input className="chat-add-dialog--button" type="submit" id="button-create" value="+" />
                </form>
                {
                    dialogs.map(dialog => (
                        <DialogElement
                            key={dialog.id}
                            dialog={dialog}
                            isCurrent={dialog.id === currentDialog?.id}
                            onClick={onDialogClick}
                        />
                    ))
                }
            </ul>
            <ChatMessages
                key={currentDialog?.id}
                dialog={currentDialog}
                onEmptyDelete={submitDeleteDialog}
                onAddMessage={onAddMessage}
                onDeleteMessages={onDeleteMessages}
                onShowDialogs={onShowDialogs}
            />
        </main>
    )
}
