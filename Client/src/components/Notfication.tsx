interface Props {
    title:string
    message:string
}

export default function Notification({message, title}:Props){
    return(<div className="notification-block-style">
        <div className="text-center">
            <b>{title}</b>
            <p>{message}</p>
        </div>
    </div>)
}