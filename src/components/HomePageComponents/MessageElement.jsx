import React from 'react'

export default function MessageElement({ mainUsername, message, onClick }) {
    var messageClass = message.authorName === mainUsername ? "message-right" : "message-left"
    if (message.isSelected) {
        messageClass += ' selected-message'
    }
    let date = new Date(message.date);
    let sendTime = `${date.getHours()}:${date.getMinutes()}`
    
    return (
        <div className={messageClass} onClick={() => onClick(message)}>
            <p>{message.text}</p>
            <time className="hint">{sendTime}</time>
        </div>
    )
}
