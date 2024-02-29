import { MouseEvent } from "react"

interface Props {
    children:string,
    onClick: (e?:MouseEvent) => void
    type: "minor" | "active" | "danger"
}

export function Button({children, onClick, type}:Props) {
    return(
        <button 
        className={`btn ${type}-btn m-1`} 
        onClick={onClick}>
            {children}
        </button>
    )
}