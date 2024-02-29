import Notification from "./Notification"

interface Props {
    title:string
    message:string
}

export default function ErrorNotification({message, title}:Props) {
    return(<Notification title={title} message={message} type="error"></Notification>)
}