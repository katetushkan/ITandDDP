import React from 'react'

export default function DialogElement({ dialog, onClick, isCurrent }) {
    let date = new Date(dialog.lastDate);
    let lastDate = `${date.getHours()}:${date.getMinutes()}`
    let lastMessage = dialog?.messages.length !== 0 ? dialog.messages.at(-1).text.substring(0, 20) : 'no messages yet'
    let extraClassName = isCurrent ? ' selected-dialog' :'';

    return (
        <div className={'chat-list--body-element' + extraClassName} onClick={() => onClick(dialog.id)}>
            <div className="chat-list--name-message">
                <p>{dialog.secondUsername}</p>
                <small className="hint">{lastMessage}</small>
            </div>
            <time className="hint">{lastDate}</time>
        </div>
    )
}
