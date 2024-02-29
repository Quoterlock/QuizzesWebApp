import { MouseEvent } from "react"

interface Props {
    iconSrc:string,
    onClick: (e:MouseEvent) => void
}

export function IconButton({iconSrc, onClick}:Props) {
    return(
        <button 
        className={`btn minor-btn m-1`} 
        onClick={onClick}>
            <img className="icon-img" src={iconSrc}/>
        </button>
    )
}