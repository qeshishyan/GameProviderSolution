import { useEffect, useState } from 'react'
import { useSpring } from 'react-spring'

export const useHover = () => {
  const [trigger, setTrigger] = useState(false)
  useEffect(() => {}, [trigger])

  const hoverStyles = useSpring({
    to: {
      transform: trigger ? 'scale(1.2)' : 'scale(1)',
      boxShadow: `0px 0px 0px 0px rgba(10px 10px 10px vars.$neonBlue)`
    },
    from: {
      transform: `scale(1)`,
      boxShadow: `0px 0px 0px 0px rgba(10px 10px 10px vars.$neonBlue)`
    },
    config: { tension: 300, friction: 10 }
  })

  const hoverOver = () => {
    setTrigger(true)
  }

  const hoverLeave = () => {
    setTrigger(false)
  }

  return { hoverStyles, hoverOver, hoverLeave }
}
