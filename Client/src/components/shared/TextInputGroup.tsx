import { ChangeEvent } from "react"

interface Props {
    onChange: (e:ChangeEvent<HTMLInputElement>) => void,
    value: string
    name: string
    label: string
    isCorrect: boolean
}


export default function TextInputGroup({onChange, value, label, name, isCorrect}:Props) {
    return(<div className="d-grid form-inputs m-1">
        <label>{label}</label>
        <input type="text" 
            className={ !isCorrect ? "wrong-input":"" } 
            onChange={onChange} 
            value={value} 
            name={name}/>
    </div>)
}