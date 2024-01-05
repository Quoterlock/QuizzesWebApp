interface Props {
    title:string
    message:string
    type?:string
}

export default function Notification({message, title, type}:Props){
    if(type === undefined) {
        type = "default"
    }
    return(<div className={`${type}-notification-block-style`}>
        <div className="text-center">
            <b>{title}</b>
            <p>{message}</p>
        </div>
    </div>)
}